using Blockchain_Indexer.Blockchain.Addresses;
using Blockchain_Indexer.Blockchain.Transactions;
using Blockchain_Indexer.Blockhain;
using Blockchain_Indexer.Repositories;
using Blockchain_Indexer.Services;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.Web3;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace Blockchain_Indexer.Blockchain;

internal class Indexer
{
    public delegate Task NewBlock(Nethereum.RPC.Eth.DTOs.Block block);
    public delegate Task NewPendingTransaction(string hash);
    public delegate Task ReindexStatus();

    public static event ReindexStatus? OnReindexingEnded;
    public static event NewPendingTransaction? OnNewPendingTransaction;
    public static event NewBlock? OnNewBlockCreated;

    private readonly List<BlockWithTransactions> _blocksToIndex = [];
    private readonly ILogger<Indexer> _logger;
    private readonly Web3 _web3;
    private bool _isIndexing;
    private readonly PendingTransactionsStorage _pendingTransactions;
    private readonly IConfiguration appConfig;
    private readonly TransactionsRepositoryFactory _repositoryFactory;

    public Indexer(ILogger<Indexer> logger,PendingTransactionsStorage pendingTransactionsStorage,IConfiguration appConfig, TransactionsRepositoryFactory repoFactory)
    {
        _repositoryFactory = repoFactory;
        _logger = logger;
        _pendingTransactions = pendingTransactionsStorage;
        this.appConfig = appConfig;
        _web3 = new(appConfig["rpc"]);
        OnNewPendingTransaction += Indexer_OnNewPendingTransaction;
        OnNewBlockCreated += Indexer_OnNewBlockCreated;
    }

    private async Task Indexer_OnNewBlockCreated(Nethereum.RPC.Eth.DTOs.Block block)
    {
        var blockWithTransactions =
            await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new BlockParameter(block.Number));
        _logger.LogInformation($"New block, hash: {block.BlockHash}");
        if (blockWithTransactions.TransactionCount() > 0)
        {
            foreach (var tx in blockWithTransactions.Transactions)
            {
                await _pendingTransactions.RemovePendingTransactionAsync(tx.TransactionHash);
            }

            _logger.LogInformation($"New block with transactions: {blockWithTransactions.Number}");
        }
        await ProcessBlockAsync(blockWithTransactions);
    }

    private async Task Indexer_OnNewPendingTransaction(string hash)
    {
        var tx = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(hash); 
        await _pendingTransactions.AddPendingTransaction(new PendingTransaction(tx));
    }

    public async Task ReindexBlocksAsync(int startBlock = 3000, int batchSize = 150)
    {
        new Thread(async () => await Reindex()).Start();
        await Task.CompletedTask;
        return;

        async Task Reindex()
        {
            _isIndexing = true;
            new Thread(async () => await BlocksListIndexer()).Start();
            Web3 web3 = new(appConfig["rpc"]);
            var endBlock = int.Parse((await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).ToString());
            _logger.LogInformation($"Latest block: {endBlock}");

            for (var i = startBlock; i < endBlock; i += batchSize)
            {
                _logger.LogInformation($"Block: {i}");

                var tasks = Enumerable.Range(i, Math.Min(batchSize, endBlock - i)).Select(async j =>
                {
                    try
                    {
                        var blockInfo =
                            await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(
                                new BlockParameter(new HexBigInteger(j)));

                        var transactionCount = blockInfo.TransactionCount();

                        if (transactionCount is 0)
                            return;

                        _blocksToIndex.Add(blockInfo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing block {j}");
                        await using var repo = _repositoryFactory.Create();
                        await repo.Errors.AddAsync(new ProcessingError(ErrorType.BlockProcessing,
                            $"Error processing block {j}"));
                        await repo.SaveChangesAsync();
                    }
                });

                await Task.WhenAll(tasks);
                _isIndexing = false;
            }

            OnReindexingEnded?.Invoke();
        }
    }

    private async Task ProcessBlockAsync(BlockWithTransactions block)
    {
        await using var repo = _repositoryFactory.Create();
        var newBlock = new Block(block);

        var findedBlock = await repo.Blocks.FindAsync(int.Parse(block.Number.ToString()));

        if (findedBlock == null)
        {
            await repo.Blocks.AddAsync(newBlock);
            _logger.LogInformation("Blocks " + block.Number + " saved");
        }
        else
        {
            repo.Blocks.Entry(findedBlock!).CurrentValues.SetValues(newBlock);
            _logger.LogInformation($"Block {block.Number} rewrited");
        }

        await repo.SaveChangesAsync();

        foreach (var tx in block.Transactions) await ProcessTransactionAsync(tx);
    }

    private async Task ProcessTransactionAsync(Transaction transaction)
    {
        //Деплой контракта
        if (string.IsNullOrEmpty(transaction.To))
        {
            await ContractDeploymentTransaction(transaction);
            return;
        }

        await using var repo = _repositoryFactory.Create();
        //Взаимодействие с контрактом
        if (await repo.ContractContains(transaction.To))
        {
            await ContractInteractionTransaction(transaction);
            return;
        }

        await TransferTransaction(transaction);
    }

    private async Task TransferTransaction(Transaction transaction)
    {
        await using var repo = _repositoryFactory.Create();

        await ProcessAddressType(transaction.From);
        await ProcessAddressType(transaction.To);

        var receipt =
            await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction.TransactionHash);

        if (receipt is null)
        {
            await repo.Errors.AddAsync(new ProcessingError(ErrorType.TransactionReceipt,
                transaction.TransactionHash));
            Transactions.Transaction tx = new(transaction);
            await repo.AddNewTransaction(tx);
            await repo.SaveChangesAsync();
            return;
        }

        await repo.UpdateWalletsAfterTransaction(receipt!);
        Transactions.Transaction newTx = new(transaction, receipt!.HasErrors()!.Value);
        await repo.AddNewTransaction(newTx);

#if DEBUG
            _logger.LogInformation("Reverted Transaction");
            _logger.LogInformation($"From:{transaction.From}");
            _logger.LogInformation($"To:{transaction.To}");
#endif

        await repo.SaveChangesAsync();
    }
    
    private async Task ProcessAddressType(string address)
    {
        var type = await Transactions.Transaction.IsWalletOrContract(address,appConfig["rpc"]);
        await using var repo = _repositoryFactory.Create();
        switch (type)
        {
            case AddressType.Wallet:
                await repo.AddNewWallet(address);
                break;

            case AddressType.Contract:
                await repo.AddNewContract(address);
                break;
        }
    }

    private async Task ContractInteractionTransaction(Transaction transaction)
    {
        await using var repo = _repositoryFactory.Create();
        var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction.TransactionHash);
        if (receipt != null)
        {
#if DEBUG
            _logger.LogInformation("New contract interaction");
#endif
            if (receipt.HasErrors()!.Value)
            {
                Transactions.Transaction tx = new(transaction, true, isContractInteraction: true);
                await repo.AddNewTransaction(tx);
                await repo.UpdateWalletsAfterTransaction(WalletUpdateType.From, receipt);
                await repo.SaveChangesAsync();
            }
            else
            {
                Transactions.Transaction tx = new(transaction, isContractInteraction: true);
                await repo.AddNewTransaction(tx);
                await repo.UpdateWalletsAfterTransaction(receipt);
                await repo.SaveChangesAsync();
            }
        }
        else
        {
            Transactions.Transaction tx = new(transaction, isContractInteraction: true);
            await repo.AddNewTransaction(tx);
            await repo.Errors.AddAsync(new ProcessingError(ErrorType.TransactionReceipt, transaction.TransactionHash));
            await repo.SaveChangesAsync();
        }
    }

    private async Task ContractDeploymentTransaction(Transaction transaction)
    {
        await using var repo = _repositoryFactory.Create();

        var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction.TransactionHash);
        if (receipt != null)
        {
#if DEBUG
            _logger.LogInformation($"New Contract address: {receipt.ContractAddress}");
#endif
            if (receipt.HasErrors()!.Value)
            {
                Transactions.Transaction tx = new(transaction, true, true);
                await repo.AddNewTransaction(tx);

                await repo.UpdateWalletsAfterTransaction(WalletUpdateType.From, receipt);
                await repo.SaveChangesAsync();
            }
            else
            {
                Transactions.Transaction tx = new(transaction, isContractDeployment: true);
                await repo.AddNewTransaction(tx);

                await repo.AddNewContract(receipt.ContractAddress, receipt.From, receipt.TransactionHash);

                await repo.UpdateWalletsAfterTransaction(receipt);
                await repo.SaveChangesAsync();
            }
        }
        else
        {
            Transactions.Transaction tx = new(transaction, isContractDeployment: true);
            await repo.AddNewTransaction(tx);
            await repo.Errors.AddAsync(new ProcessingError(ErrorType.TransactionReceipt, transaction.TransactionHash));
            await repo.SaveChangesAsync();
        }
    }

    public async Task NewPendingTransactions()
    {
        new Thread(async () => await Listening()).Start();
        await Task.CompletedTask;
        return;

        async Task Listening()
        {
            var client = new StreamingWebSocketClient(appConfig["ws"]);
            // create the subscription
            // it won't start receiving data until Subscribe is called on it
            var subscription = new EthNewPendingTransactionObservableSubscription(client);

            // attach a handler subscription created event (optional)
            // this will only occur once when Subscribe has been called
            subscription.GetSubscribeResponseAsObservable().Subscribe(subscriptionId =>
                _logger.LogInformation("Pending transactions subscription Id: " + subscriptionId));

            // attach a handler for each pending transaction
            // put your logic here
            subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async transactionHash =>
            {
                OnNewPendingTransaction?.Invoke(transactionHash);
            });

            var subscribed = true;

            //handle unsubscription
            //optional - but may be important depending on your use case
            subscription.GetUnsubscribeResponseAsObservable().Subscribe(response =>
            {
                subscribed = false;
                _logger.LogInformation($"Pending transactions unsubscribe result: {response}");
            });

            //open the websocket connection
            await client.StartAsync();

            // start listening for pending transactions
            // this will only block long enough to register the subscription with the client
            // it won't block whilst waiting for transactions
            // transactions will be delivered to our handlers on another thread
            await subscription.SubscribeAsync();

            // run for minute
            // transactions should appear on another thread
            //await Task.Delay(TimeSpan.FromMinutes(1));

            // unsubscribe
            //await subscription.UnsubscribeAsync();

            // wait for unsubscribe
            while (subscribed) await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public async Task NewBlockHeader()
    {
        new Thread(async () => await Listening()).Start();
        await Task.CompletedTask;
        return;

        async Task Listening()
        {
            using var client = new StreamingWebSocketClient(appConfig["ws"]);
            // create the subscription
            // (it won't start receiving data until Subscribe is called)
            var subscription = new EthNewBlockHeadersObservableSubscription(client);

            // attach a handler for when the subscription is first created (optional)
            // this will occur once after Subscribe has been called
            subscription.GetSubscribeResponseAsObservable().Subscribe(subscriptionId =>
                _logger.LogInformation("Block Header subscription Id: " + subscriptionId));

            // attach a handler for each block
            // put your logic here
            subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(block =>
            {
                OnNewBlockCreated?.Invoke(block);
            });

            var subscribed = true;

            // handle unsubscription
            // optional - but may be important depending on your use case
            subscription.GetUnsubscribeResponseAsObservable().Subscribe(response =>
            {
                subscribed = false;
                _logger.LogInformation($"Block Header unsubscribe result: {response}");
            });

            // open the websocket connection
            await client.StartAsync();

            // start the subscription
            // this will only block long enough to register the subscription with the client
            // once running - it won't block whilst waiting for blocks
            // blocks will be delivered to our handler on another thread
            await subscription.SubscribeAsync();

            // run for a minute before unsubscribing
            //await Task.Delay(TimeSpan.FromMinutes(1));

            // unsubscribe
            //await subscription.UnsubscribeAsync();

            //allow time to unsubscribe
            while (subscribed) await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    private async Task BlocksListIndexer()
    {
        while (_isIndexing)
            if (_blocksToIndex.Count > 10)
            {
                _logger.LogInformation($"Blocks in queue {_blocksToIndex.Count}");

                for (var i = 0; i < 10; i++)
                {
                    await ProcessBlockAsync(_blocksToIndex[i]);
                    _blocksToIndex.RemoveAt(i);
                }
            }
    }
}