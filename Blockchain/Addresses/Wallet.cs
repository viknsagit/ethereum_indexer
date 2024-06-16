using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain_Indexer.Blockchain.Addresses;

public class Wallet()
{
    public Wallet(string address) : this()
    {
        Address = address;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Key] public string Address { get; private set; }

    public int TransactionsCount { get; set; }
    public decimal GasUsed { get; set; }
    public int LastBalanceUpdate { get; set; }
}