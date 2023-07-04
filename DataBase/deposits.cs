namespace CodeRunners.Database
{
    public class Deposit
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; } 
        public double Amount { get; set; }
        public DateTime Date { get; set; } 
        public string Status { get; set; } 
        public int StripeId { get; set; } 
    }
}