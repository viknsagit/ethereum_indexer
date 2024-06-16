using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain_Indexer.Blockchain.Contracts;

public class Token()
{
    public Token(string address, string name, string symbol, decimal supply, int holders, TokenType type) : this()
    {
        Contract = address;
        Name = name;
        Symbol = symbol;
        Supply = supply;
        TokenType = type;
        Holders = holders;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Key] public string Contract { get; private set; }

    public string Name { get; private set; }
    public string Symbol { get; private set; }
    public decimal Supply { get; private set; }
    public int Holders { get; set; }

    public TokenType TokenType { get; set; }
}