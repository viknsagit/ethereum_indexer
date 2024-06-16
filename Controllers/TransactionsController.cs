using Blockchain_Indexer.Repositories;
using Blockchain_Indexer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Indexer.Controllers;

[Route("/api/[controller]")]
public class TransactionsController : Controller
{
    [HttpGet("latestHashes")]
    public async Task<IActionResult> GetLatestTransactionsHashes([FromServices] TransactionsRepositoryFactory repoFactory,int count = 10)
    {
                await using var repo = repoFactory.Create();

        var txs = new string[count];

        var latestTx = await repo.Transactions.OrderByDescending(t => t.Number).FirstOrDefaultAsync()!;
        txs[0] = latestTx!.Hash;

        for (var i = 1; i < count; i++)
            txs[i] = (await repo.Transactions.OrderByDescending(t => t.Number == latestTx.Number - i)
                .FirstOrDefaultAsync()!)!.Hash;
        return Ok(txs);
    }

    [HttpGet("latestTxs")]
    public async Task<IActionResult> GetLatestTransactions([FromServices] TransactionsRepositoryFactory repoFactory,int page = 1)
    {
        if (page < 1) page = 1;

                await using var repo = repoFactory.Create();

        var transactions = await repo.Transactions
            .OrderByDescending(t => t.Number)
            .Skip((page - 1) * 15)
            .Take(15)
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpGet("latestBlocks")]
    public async Task<IActionResult> GetLatestBlocks([FromServices] TransactionsRepositoryFactory repoFactory,int page = 1)
    {
        if (page < 1) page = 1;

                await using var repo = repoFactory.Create();

        var blocks = await repo.Blocks
            .OrderByDescending(b => b.BlockNumber)
            .Skip((page - 1) * 15)
            .Take(15)
            .ToListAsync();

        return Ok(blocks);
    }

    [HttpGet("byHash")]
    [OutputCache(Duration = 5600)]
    public async Task<IActionResult> GetTransactionByHash([FromServices] TransactionsRepositoryFactory repoFactory,string hash)
    {
                await using var repo = repoFactory.Create();

        var tx = await repo.Transactions.Where(t => t.Hash == hash).FirstOrDefaultAsync();
        return Ok(tx);
    }

    [HttpGet("txsCount")]
    public async Task<IActionResult> GetTransactionsCount([FromServices] TransactionsRepositoryFactory repoFactory)
    {
                await using var repo = repoFactory.Create();

        var tx = await repo.Transactions.OrderByDescending(t => t.Number).FirstOrDefaultAsync();
        return Ok(tx!.Number);
    }

    [HttpGet("pendingTxs")]
    [OutputCache(Duration = 15)]
    public async Task<IActionResult> GetPendingTransactions([FromServices] PendingTransactionsStorage storage)
    {
        return Ok(await storage.GetPendingTransactionsAsync());
    }

    [HttpGet("txsHistory")]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetLastSevenDaysTransactionsHistoryAsync([FromServices] TransactionsRepositoryFactory repoFactory)
    {
                await using var repo = repoFactory.Create();

        var transactionsHistory = await repo.GetLastSevenDaysTransactionsHistoryAsync();
        return Ok(transactionsHistory);
    }
}