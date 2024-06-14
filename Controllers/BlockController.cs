using System.Collections.Immutable;
using Blockchain_Indexer.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Blockchain_Indexer.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BlockController : Controller
    {
        [HttpGet("latestBlock")]
        public async Task<IActionResult> GetLatestBlockNumber([FromServices] IConfiguration config)
        {
            var web3 = new Web3(config["rpc"]);
            var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            return Ok(latestBlockNumber.Value.ToString());
        }

        [HttpGet("txsByBlock")]
        [OutputCache(Duration = 5600)]
        public async Task<IActionResult> GetTransactionsByBlockNumber(int block, int pageIndex = 1)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            await using TransactionsRepository repo = new();
            var txs = repo.Transactions
                .Where(t => t.Block == block)
                .OrderByDescending(t => t.Timestamp)
                .Skip((pageIndex - 1) * 15)
                .Take(15)
                .ToImmutableList();

            if (txs.Count > 0)
                return Ok(txs);
            return BadRequest($"block {block} doesnt have transactions");
        }

        [HttpGet("txsCount")]
        public async Task<IActionResult> GetTransactionsCountByBlock([FromServices] IConfiguration config,int blockNumber)
        {
            var web3 = new Web3(config["rpc"]);
            var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(blockNumber));
            return Ok(block.TransactionCount());
        }

        [HttpGet("block")]
        public async Task<IActionResult> GetBlockAsync([FromServices] IConfiguration config, int blockNumber)
        {
            await using TransactionsRepository repo = new();
            var block = await repo.Blocks.FindAsync(blockNumber);

            if (block is not null) return Ok(block);

            var web3 = new Web3(config["rpc"]);
            block = new Block(await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new BlockParameter(new HexBigInteger(blockNumber))));

            return Ok(block);

        }

    }
}
