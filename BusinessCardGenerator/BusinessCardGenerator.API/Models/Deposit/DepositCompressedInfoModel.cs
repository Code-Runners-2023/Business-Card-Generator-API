using System.ComponentModel.DataAnnotations.Schema;
using DepositClass = BusinessCardGenerator.API.Data.Deposit;

namespace BusinessCardGenerator.API.Models.Deposit
{
    public class DepositCompressedInfoModel
    {
        public DepositCompressedInfoModel(DepositClass deposit)
        {
            Id = deposit.Id;
            Amount = deposit.Amount;
            Date = deposit.Date;
            Status = deposit.Status;
            StripeId = deposit.StripeId;
        } 

        public Guid Id { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string StripeId { get; set; }
    }
}
