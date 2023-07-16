using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardGenerator.API.Models.Transaction
{
    public class TransactionInputModel
    {
        public Guid BusinessCardId { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
