using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blockchain_Indexer.Blockhain;

namespace Blockchain_Indexer;

public class ProcessingError()
{
    public ProcessingError(ErrorType type, string hash) : this()
    {
        Type = type;
        TransactionHash = hash;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public ProcessingError(ErrorType type, string hash, string msg) : this()
    {
        Type = type;
        TransactionHash = hash;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        Message = msg;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string TransactionHash { get; private set; }

    public ErrorType Type { get; private set; }

    public long Timestamp { get; private set; }

    public string? Message { get; private set; }
}