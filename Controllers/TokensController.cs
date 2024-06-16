using Blockchain_Indexer.Blockchain.Contracts;
using Blockchain_Indexer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Indexer.Controllers;

[Route("/api/[controller]")]
public class TokensController : Controller
{
    [HttpGet("token")]
    [OutputCache(Duration = 160)]
    public async Task<IActionResult> GetTokenByHashAsync([FromServices] TransactionsRepositoryFactory repoFactory,string hash)
    {
                await using var repo = repoFactory.Create();

        var token = await repo.Tokens.Where(t => t.Contract == hash).FirstOrDefaultAsync();
        if (token == null)
            return NotFound();
        return Ok(token);
    }

    [HttpGet("tokens")]
    [OutputCache(Duration = 165)]
    public async Task<IActionResult> GetTokensHashesListAsync([FromServices] TransactionsRepositoryFactory repoFactory,int page = 1)
    {
        const int pageSize = 15;
        if (page < 1) page = 1;

                await using var repo = repoFactory.Create();

        var tokens = await repo.Tokens
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(tokens);
    }

    [HttpGet("tokensCount")]
    public async Task<IActionResult> GetTokensCountAsync([FromServices] TransactionsRepositoryFactory repoFactory)
    {
                await using var repo = repoFactory.Create();

        var count = await repo.Tokens.CountAsync();
        return Ok(count);
    }
}