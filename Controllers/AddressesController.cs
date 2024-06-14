using Blockchain_Indexer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Nethereum.Web3;

namespace Blockchain_Indexer.Controllers;

[Route("/api/[controller]")]
public class AddressesController : Controller
{
    [HttpGet("address")]
    [OutputCache(Duration = 60)]
    public async Task<IActionResult> GetAddressAsync(string address)
    {
        address = address.ToLower();
        await using TransactionsRepository repo = new();
        var wallet = await repo.Addresses.Where(x => x.Address == address).FirstOrDefaultAsync();
        if (wallet == null)
            return BadRequest();
        return Ok(wallet);
    }


    [HttpGet("balance")]
    public async Task<IActionResult> GetBalanceAsync([FromServices] IConfiguration config, string address)
    {
        var web3 = new Web3(config["rpc"]);
        var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
        return Ok(balance.Value.ToString());
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetAddressLatestTransactions(string address, int page = 1)
    {
        if (page < 1) page = 1;

        address = address.ToLower();
        await using TransactionsRepository repo = new();
        var transactions = await repo.Transactions
            .Where(x => x.FromAddress == address || x.ToAddress == address)
            .OrderByDescending(t => t.Timestamp)
            .Skip((page - 1) * 15)
            .Take(15)
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpGet("tokens")]
    [OutputCache(Duration = 30)]
    public async Task<IActionResult> GetAddressTokensAsync(string address, int page = 1)
    {
        if (page < 1) page = 1;

        address = address.ToLower();
        await using TransactionsRepository repo = new();
        var tokens = await repo.TokenHolders
            .Where(x => x.Owner == address)
            .Skip((page - 1) * 15)
            .Take(15)
            .ToListAsync();

        return Ok(tokens);
    }

    [HttpGet("transactionCount")]
    [OutputCache(Duration = 30)]
    public async Task<IActionResult> GetTransactionsCountAsync(string address)
    {
        address = address.ToLower();
        await using TransactionsRepository repo = new();
        var txsFrom = await repo.Transactions.Where(x => x.FromAddress == address).CountAsync();
        var txsTo = await repo.Transactions.Where(x => x.ToAddress == address).CountAsync();
        return Ok(txsFrom + txsTo);
    }
}