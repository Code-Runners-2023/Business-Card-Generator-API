using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardGenerator.API.Data
{
    public class Deposit
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public double Amount { get; set; }
        
        public DateTime Date { get; set; }
        
        public string Status { get; set; }
        
        public int StripeId { get; set; }
    }
}