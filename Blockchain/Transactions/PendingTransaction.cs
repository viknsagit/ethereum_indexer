using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nethereum.Web3;

namespace Blockchain_Indexer.Blockchain.Transactions;

public class PendingTransaction()
{
    public PendingTransaction(Nethereum.RPC.Eth.DTOs.Transaction tx) : this()
    {
        Hash = tx.TransactionHash;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        FromAddress = tx.From;
        ToAddress = tx.To;
        Input = tx.Input;
        Gas = Web3.Convert.FromWei(tx.Gas);
        GasPrice = Web3.Convert.FromWei(tx.GasPrice);
        Value = Web3.Convert.FromWei(tx.Value);
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Number { get; }

    [Key] public string Hash { get; private set; }

    public string Input { get; private set; }

    public string FromAddress { get; private set; }

    public string? ToAddress { get; private set; }

    public long Timestamp { get; private set; }
    public decimal Value { get; private set; }
    public decimal Gas { get; private set; }
    public decimal GasPrice { get; private set; }
}