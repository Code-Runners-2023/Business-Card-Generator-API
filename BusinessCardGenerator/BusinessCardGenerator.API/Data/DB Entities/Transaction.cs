using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardGenerator.API.Data
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(BusinessCard))]
        public Guid BusinessCardId { get; set; }

        public BusinessCard BusinessCard { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }
    }
}