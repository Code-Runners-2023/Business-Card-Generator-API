using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardGenerator.API.Data
{
    public class Deposit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        
        public User User { get; set; }
        
        public double Amount { get; set; }
        
        public DateTime Date { get; set; }
        
        public string Status { get; set; }
        
        public string StripeId { get; set; }
    }
}