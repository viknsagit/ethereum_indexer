using Blockchain_Indexer.Blockchain.Transactions;

namespace Blockchain_Indexer.Services
{
    public class PendingTransactionsStorage
    {
        private static readonly Dictionary<string, PendingTransaction> PendingTransactions = [];

        public async Task AddPendingTransaction(PendingTransaction tx)
        {
            PendingTransactions.Add(tx.Hash, tx);
            await Task.CompletedTask;
        }

        public async Task<PendingTransaction?> GetPendingTransactionAsync(string hash)
        {
            return await Task.FromResult(PendingTransactions.GetValueOrDefault(hash));
        }

        public async Task RemovePendingTransactionAsync(string hash)
        {
            PendingTransactions.Remove(hash);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<PendingTransaction>> GetPendingTransactionsAsync(int pageIndex = 0)
        {
            var transactions = PendingTransactions.Values
                .Skip(pageIndex * 15)
                .Take(15);
            return await Task.FromResult(transactions);
        }


    }
}
