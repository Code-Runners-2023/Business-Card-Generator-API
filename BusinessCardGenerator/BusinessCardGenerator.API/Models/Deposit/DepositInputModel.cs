namespace BusinessCardGenerator.API.Models.Deposit
{
    public class DepositInputModel
    {
        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string StripeId { get; set; }
    }
}
