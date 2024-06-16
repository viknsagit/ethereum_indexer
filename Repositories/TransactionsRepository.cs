using Blockchain_Indexer.Blockchain;
using Blockchain_Indexer.Blockchain.Addresses;
using Blockchain_Indexer.Blockchain.Contracts;
using Blockchain_Indexer.Blockchain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System.Globalization;

using Transaction = Blockchain_Indexer.Blockchain.Transactions.Transaction;

namespace Blockchain_Indexer.Repositories;

public class TransactionsRepository : DbContext
{
    public DbSet<ProcessingError> Errors => Set<ProcessingError>();
    public DbSet<Block> Blocks => Set<Block>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    private DbSet<PendingTransaction> PendingTransactions => Set<PendingTransaction>();
    public DbSet<Wallet> Addresses => Set<Wallet>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<TokenHolder> TokenHolders => Set<TokenHolder>();
    private DbSet<TransactionsHistoryDto> TransactionsHistory => Set<TransactionsHistoryDto>();

    public TransactionsRepository(DbContextOptions<TransactionsRepository> options)
        : base(options)
    {
    }

    public async Task<List<TransactionsHistoryDto>> GetLastSevenDaysTransactionsHistoryAsync()
    {
        var today = DateTime.Today;
        var sevenDaysAgo = today.AddDays(-7);
        var transactionsHistory = TransactionsHistory
            .AsEnumerable()
            .Where(th => DateTime.ParseExact(th.DateNameId, "dd-MM-yyyy", CultureInfo.InvariantCulture) >= sevenDaysAgo
                         && DateTime.ParseExact(th.DateNameId, "dd-MM-yyyy", CultureInfo.InvariantCulture) <= today)
            .OrderByDescending(th => DateTime.ParseExact(th.DateNameId, "dd-MM-yyyy", CultureInfo.InvariantCulture))
            .Take(7)
            .ToList();
        return await Task.FromResult(transactionsHistory);
    }
    public async Task<List<SearchResultType>> SearchAsync(string address)
    {
        List<SearchResultType> results =
        [
            await Transactions.FindAsync(address) != null ? SearchResultType.Transaction : SearchResultType.Null,
            await Contracts.FindAsync(address) != null ? SearchResultType.Contract : SearchResultType.Null,
            await Tokens.FindAsync(address) != null ? SearchResultType.Token : SearchResultType.Null,
            await Addresses.FindAsync(address) != null ? SearchResultType.Address : SearchResultType.Null,
            await Blocks.FindAsync(address) != null ? SearchResultType.Block : SearchResultType.Null
        ];

        results.RemoveAll(item => item == SearchResultType.Null);
        return results;
    }
    public async Task CreateNewDateInHistory()
    {
        var date = DateTime.Now.ToString("dd-MM-yyyy");
        var finded = await TransactionsHistory.FindAsync(date);
        if (finded is null)
        {
            TransactionsHistoryDto historyDto = new()
            {
                DateNameId = date,
                TransactionsCount = 0
            };
            await TransactionsHistory.AddAsync(historyDto);
        }
    }

    public async Task CreateTransactionsHistoryUntilEndOfYear()
    {
        var today = DateTime.Today;
        var endOfYear = new DateTime(today.Year, 12, 31);

        for (var date = today; date <= endOfYear; date = date.AddDays(1))
        {
            var dateNameId = date.ToString("dd-MM-yyyy");
            var finded = await TransactionsHistory.FindAsync(dateNameId);
            if (finded is not null) continue;
            TransactionsHistoryDto historyDto = new()
            {
                DateNameId = dateNameId,
                TransactionsCount = 0
            };
            await TransactionsHistory.AddAsync(historyDto);
        }
        await SaveChangesAsync();
    }


    private async Task IncreaseTransactionsInHistory()
    {
        var date = DateTime.Now.ToString("dd-MM-yyyy");
        var finded = await TransactionsHistory.FindAsync(date);
        if (finded != null)
        {
            var newHistory = finded!;
            newHistory.TransactionsCount++;
            TransactionsHistory.Entry(finded).CurrentValues.SetValues(newHistory);
        }
    }

    public async Task AddPendingTransaction(PendingTransaction tx)
    {
        await PendingTransactions.AddAsync(tx);
    }

    public async Task<PendingTransaction?> GetPendingTransactionAsync(string hash)
    {
        var tx = await PendingTransactions.FindAsync(hash);
        return tx;
    }

    public async Task AddTokenHolder(TokenHolder tokenHolder)
    {
        var finded = await TokenHolders.FindAsync(tokenHolder);
        if (finded is null)
            await TokenHolders.AddAsync(tokenHolder);
        else
            TokenHolders.Entry(finded).CurrentValues.SetValues(tokenHolder);
    }

    public async Task<Transaction?> GetTransactionAsync(string hash)
    {
        var tx = await Transactions.FindAsync(hash);
        return tx;
    }

    public async Task<bool> WalletContains(string address)
    {
        var result = await Addresses.FindAsync(address);
        return result is not null;
    }

    public async Task<bool> ContractContains(string address)
    {
        var result = await Contracts.FindAsync(address);
        return result is not null;
    }

    public async Task RemovePendingTransactionAsync(string hash)
    {
        var finded = await GetPendingTransactionAsync(hash);
        if (finded != null)
            PendingTransactions.Remove(finded);
    }

    public async Task AddNewTransaction(Transaction tx)
    {
        await Transactions.AddAsync(tx);
        await IncreaseTransactionsInHistory();
    }

    public async Task AddNewWallet(string address)
    {
        var finded = await Addresses.FindAsync(address);
        if (finded is null)
        {
            Wallet wallet = new(address);
            await Addresses.AddAsync(wallet);
        }
    }

    public async Task UpdateWalletsAfterTransaction(TransactionReceipt tx)
    {
        var fromFinded = await Addresses.FindAsync(tx.From);

        if (fromFinded != null)
        {
            var newFrom = fromFinded!;
            newFrom.TransactionsCount++;
            newFrom.LastBalanceUpdate = int.Parse(tx.BlockNumber.ToString());
            newFrom.GasUsed += Web3.Convert.FromWei(tx.GasUsed);
            Addresses.Entry(fromFinded!).CurrentValues.SetValues(newFrom);
        }

        if (!string.IsNullOrEmpty(tx.To))
        {
            var fromTo = await Addresses.FindAsync(tx.To);

            if (fromTo != null)
            {
                var newTo = fromTo!;
                newTo.TransactionsCount++;
                newTo.LastBalanceUpdate = int.Parse(tx.BlockNumber.ToString());
                Addresses.Entry(fromTo!).CurrentValues.SetValues(newTo);
            }
        }
    }

    public async Task UpdateWalletsAfterTransaction(WalletUpdateType type, TransactionReceipt tx)
    {
        switch (type)
        {
            case WalletUpdateType.From:
            {
                var fromFinded = await Addresses.FindAsync(tx.From);

                if (fromFinded != null)
                {
                    var newFrom = fromFinded!;
                    newFrom.TransactionsCount++;
                    newFrom.LastBalanceUpdate = int.Parse(tx.BlockNumber.ToString());
                    newFrom.GasUsed += Web3.Convert.FromWei(tx.GasUsed);
                    Addresses.Entry(fromFinded!).CurrentValues.SetValues(newFrom);
                }
            }
                break;

            case WalletUpdateType.To:
            {
                if (!string.IsNullOrEmpty(tx.To))
                {
                    var fromTo = await Addresses.FindAsync(tx.From);

                    if (fromTo != null)
                    {
                        var newTo = fromTo!;
                        newTo.TransactionsCount++;
                        newTo.LastBalanceUpdate = int.Parse(tx.BlockNumber.ToString());
                        Addresses.Entry(fromTo!).CurrentValues.SetValues(newTo);
                    }
                }
            }
                break;
        }
    }

    public async Task AddNewContract(string address, string creator, string hash)
    {
        var finded = await Contracts.FindAsync(address);
        if (finded is null)
        {
            Contract contract = new(address, creator, hash);
            await Contracts.AddAsync(contract);
        }
    }

    public async Task AddNewContract(string address)
    {
        var finded = await Contracts.FindAsync(address);
        if (finded is null)
        {
            Contract contract = new(address);
            await Contracts.AddAsync(contract);
        }
    }
}