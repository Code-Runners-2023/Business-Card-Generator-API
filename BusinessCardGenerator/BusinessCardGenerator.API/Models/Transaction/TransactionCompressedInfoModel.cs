using System.ComponentModel.DataAnnotations.Schema;
using TransactionClass = BusinessCardGenerator.API.Data.Transaction;

namespace BusinessCardGenerator.API.Models.Transaction
{
    public class TransactionCompressedInfoModel
    {
        public TransactionCompressedInfoModel(TransactionClass transaction)
        {
            Id = transaction.Id;
            BusinessCardId = transaction.BusinessCardId;
            Amount = transaction.Amount;
            Date = transaction.Date;
        }

        public Guid Id { get; set; }

        public Guid BusinessCardId { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
