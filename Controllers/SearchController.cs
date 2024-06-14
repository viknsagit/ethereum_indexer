using Blockchain_Indexer.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Indexer.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class SearchController : Controller
    {

        [HttpGet("searchAll")]
        [OutputCache(Duration = 5600)]
        public async Task<IActionResult> SearchAsync(string address)
        {
            await using TransactionsRepository repo = new();
            var result = await repo.SearchAsync(address);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("searchTransaction")]
        [OutputCache(Duration = 5600)]
        public async Task<IActionResult> SearchTransaction(string txHash)
        {
            await using TransactionsRepository repo = new();
            var result = await repo.Transactions.Where(t => t.Hash == txHash)
                .FirstOrDefaultAsync();
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("searchAddress")]
        [OutputCache(Duration = 5600)]
        public async Task<IActionResult> SearchAddress(string address)
        {
            await using TransactionsRepository repo = new();
            var result = await repo.Addresses.Where(t => t.Address == address)
                .FirstOrDefaultAsync();
            return result is null ? NotFound() : Ok(result);
        }


        [HttpGet("searchToken")]
        [OutputCache(Duration = 5600)]
        public async Task<IActionResult> SearchToken(string address)
        {
            await using TransactionsRepository repo = new();
            var result = await repo.Tokens.Where(t => t.Contract == address)
                .FirstOrDefaultAsync();
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("searchContract")]
        [OutputCache(Duration = 5600)]
        public async Task<IActionResult> SearchContract(string address)
        {
            await using TransactionsRepository repo = new();
            var result = await repo.Contracts.Where(t => t.Address == address)
                .FirstOrDefaultAsync();
            return result is null ? NotFound() : Ok(result);
        }

    }
}
