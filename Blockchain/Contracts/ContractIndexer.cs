using System.Numerics;
using Blockchain_Indexer.Blockchain.TokensFunctions;
using Blockchain_Indexer.Blockhain;
using Blockchain_Indexer.Repositories;
using Microsoft.EntityFrameworkCore;
using Nethereum.Web3;

namespace Blockchain_Indexer.Blockchain.Contracts;

public class ContractIndexer
{
    private readonly ILogger<ContractIndexer> _logger;
    private readonly Web3 _web3;
    private readonly IConfiguration config;
    private readonly TransactionsRepositoryFactory _repoFactory;

    public ContractIndexer(ILogger<ContractIndexer> logger,IConfiguration config,TransactionsRepositoryFactory repoFactory)
    {
        _repoFactory = repoFactory;
        _logger = logger;
        this.config = config;
        Indexer.OnNewBlockCreated += BlockchainIndexer_OnNewBlockCreated;
    }

    private async Task BlockchainIndexer_OnNewBlockCreated(Nethereum.RPC.Eth.DTOs.Block? block)
    {
        if (block == null)
            return;

        var blockWithThx = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(block.Number);
        foreach (var tx in blockWithThx.Transactions)
        {
            await using var repo = _repoFactory.Create();
            var tokenFrom = await repo.Contracts.FindAsync(tx.From);
            var tokenTo = await repo.Contracts.FindAsync(tx.To);
            if (tokenFrom != null)
            {
                var newFrom = tokenFrom;
                newFrom.TransactionsCount++;
                repo.Contracts.Entry(tokenFrom).CurrentValues.SetValues(newFrom);
            }

            if (tokenTo != null)
            {
                var newTo = tokenTo;
                newTo.TransactionsCount++;
                repo.Contracts.Entry(tokenTo).CurrentValues.SetValues(newTo);
            }

            await repo.SaveChangesAsync();
        }
    }

    public async Task ReindexTokensFromDatabase()
    {
        _logger.LogInformation("Start token reindexing");
        await using var repo = _repoFactory.Create();
        var lastContract = await repo.Contracts.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        for (var i = 1; i <= lastContract!.Id; i++)
        {
            var contract = await repo.Contracts.Where(x => x.Id == i).FirstOrDefaultAsync();
            if (contract == null) continue;
            var txs = await CheckContractTransactions(contract.Address);
            var newContract = contract;
            newContract.TransactionsCount = txs;
            repo.Contracts.Entry(contract).CurrentValues.SetValues(newContract);
            await repo.SaveChangesAsync();
            await CheckContractForERCToken(contract.Address);
        }
    }

    private async Task<int> CheckTokenHolders(string token)
    {
        await using var repo = _repoFactory.Create();
        var contract = _web3.Eth.GetContractHandler(token);

        var usersCount = await repo.Addresses.CountAsync();
        var TokenHolders = 0;
        for (var i = 1; i <= usersCount; i++)
        {
            var user = await repo.Addresses.Where(x => x.Id == i).FirstOrDefaultAsync();
            if (user != null)
            {
                var balance = Web3.Convert.FromWei(await contract.GetFunction<ERC20_Functions.BalanceOfFunction>()
                    .CallAsync<BigInteger>(new ERC20_Functions.BalanceOfFunction { Owner = user.Address }));
                if (balance > 0)
                {
                    TokenHolders++;
                    _logger.LogInformation($"New token holder {user.Address}");
                    await repo.AddTokenHolder(new TokenHolder(user.Address, token, balance));
                    await repo.SaveChangesAsync();
                }
            }
        }

        return TokenHolders;
    }

    private async Task<int> CheckContractTransactions(string token)
    {
        await using var repo = _repoFactory.Create();
        var fromCount = await repo.Transactions.Where(t => t.FromAddress == token).CountAsync();
        var toCount = await repo.Transactions.Where(t => t.ToAddress == token).CountAsync();
        _logger.LogInformation($"Token txs {toCount + fromCount}");
        return fromCount + toCount;
    }

    private async Task<bool> CheckContractForERCToken(string address)
    {
        var contract = _web3.Eth.GetContractHandler(address);
        try
        {
            await using var repo = _repoFactory.Create();

            var name = await contract.GetFunction<ERC20_Functions.NameFunction>().CallAsync<string>();
            var symbol = await contract.GetFunction<ERC20_Functions.SymbolFunction>().CallAsync<string>();
            var totalSupply = Web3.Convert.FromWei(await contract.GetFunction<ERC20_Functions.TotalSupplyFunction>()
                .CallAsync<BigInteger>());

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(symbol))
                return false;

            var finded = await repo.Tokens.FindAsync(address);
            if (finded is null)
            {
                //int holdersCount = await CheckTokenHolders(address);
                repo.Tokens.Add(new Token(address, name, symbol, totalSupply, 0, TokenType.ERC20));
                await repo.SaveChangesAsync();
                _logger.LogInformation($"New token {name}");
                return true;
            }

            return true;
        }
        catch (Exception)
        {
            await using var repo = _repoFactory.Create();
            await repo.Errors.AddAsync(new ProcessingError(ErrorType.TokenProcessing, address));
            await repo.SaveChangesAsync();
            return false;
        }
    }
}