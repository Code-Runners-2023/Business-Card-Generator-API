using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardGenerator.API.Data
{
    public class BusinessCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [StringLength(120, ErrorMessage = "Too long company name! Max length is 120 characters!")] 
        public string Name { get; set; }

        [StringLength(120, ErrorMessage = "Too long position! Max length is 120 characters!")]
        public string Position { get; set; }

        [StringLength(250, ErrorMessage = "Too long address! Max length is 250 characters!")]
        public string Address { get; set; }

        [Url]
        public string Website { get; set; }

        public string LogoPath { get; set; }

        [StringLength(13, ErrorMessage = "Invalid rgb color code! Max length is 13 characters!")]
        public string RGBColorCode { get; set; }
    }
}