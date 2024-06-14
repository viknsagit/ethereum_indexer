using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Indexer.Blockchain
{
    [Index("DateNameId", IsUnique = true)]
    public class TransactionsHistoryDto
    {
        [Key]
        [StringLength(11)]
        public required string DateNameId{ get; set; }
        public int TransactionsCount { get; set; }

    }
}
