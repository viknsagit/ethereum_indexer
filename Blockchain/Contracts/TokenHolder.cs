using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Indexer.Blockchain.Contracts;

[Index("TokenAddress")]
public class TokenHolder()
{
    public TokenHolder(string owner, string token, decimal value) : this()
    {
        Owner = owner;
        TokenAddress = token;
        TokenValue = value;
    }

    [Key] public string Owner { get; set; }

    public string TokenAddress { get; set; }
    public decimal TokenValue { get; set; }
}