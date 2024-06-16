using Blockchain_Indexer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Indexer.Controllers;

[Route("/api/[controller]")]
public class ContractsController : Controller
{
    [HttpGet("contract")]
    public async Task<IActionResult> GetContractByAddress([FromServices] TransactionsRepositoryFactory repoFactory, string address)
    {
        await using var repo = repoFactory.Create();
        var contract = await repo.Contracts.FindAsync(address);
        if (contract == null)
            return NotFound();
        return Ok(contract);
    }

    [HttpGet("transactionsCount")]
    public async Task<IActionResult> GetContractTransactionsByAddress([FromServices] TransactionsRepositoryFactory repoFactory,string address)
    {
        await using var repo = repoFactory.Create();

        var contract = await repo.Contracts.FindAsync(address);
        if (contract is null)
            return NotFound();
        return Ok(contract.TransactionsCount);
    }

    [HttpGet("contractsCount")]
    public async Task<IActionResult> GetContractsCountAsync([FromServices] TransactionsRepositoryFactory repoFactory)
    {
        await using var repo = repoFactory.Create();
        var count = await repo.Contracts.CountAsync();
        return Ok(count);
    }

}