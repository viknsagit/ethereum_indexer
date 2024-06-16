using System.ComponentModel.DataAnnotations;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Blockchain_Indexer;

public class Block()
{
    public Block(BlockWithTransactions block) : this()
    {
        BlockNumber = int.Parse(block.Number.ToString());
        TransactionsNumber = block.TransactionCount();
        GasLimit = Web3.Convert.FromWei(block.GasLimit);
        GasUsed = Web3.Convert.FromWei(block.GasUsed);
        Timestamp = long.Parse(block.Timestamp.ToString());
    }

    [Key] public int BlockNumber { get; set; }

    public int TransactionsNumber { get; set; }
    public decimal GasLimit { get; set; }
    public decimal GasUsed { get; set; }

    public long Timestamp { get; set; }
}