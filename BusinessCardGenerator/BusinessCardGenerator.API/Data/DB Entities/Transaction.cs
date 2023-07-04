using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardGenerator.API.Data
{
    public class Transaction
    {
        public int Id { get; set; }

        [ForeignKey(nameof(BusinessCard))]
        public int BusinessCardId { get; set; }

        public BusinessCard BusinessCard { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }
    }
}