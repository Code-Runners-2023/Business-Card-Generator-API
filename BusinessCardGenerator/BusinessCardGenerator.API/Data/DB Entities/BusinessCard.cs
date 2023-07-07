using BusinessCardGenerator.API.Models.BusinessCard;
using BusinessCardGenerator.API.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace BusinessCardGenerator.API.Data
{
    public class BusinessCard
    {
        public void ApplyChanges(BusinessCardInputModel model, string logoPath)
        {
            Name = model.Name;
            Address = model.Address;
            Website = model.Website;
            LogoPath = logoPath;
            HexColorCodeMain = model.HexColorCodeMain;
            HexColorCodeSecondary = model.HexColorCodeSecondary;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [StringLength(120, ErrorMessage = "Too long company name! Max length is 120 characters!")] 
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "Too long address! Max length is 250 characters!")]
        public string Address { get; set; }

        [Url]
        public string Website { get; set; }

        public string LogoPath { get; set; }

        public string HexColorCodeMain { get; set; }

        public string HexColorCodeSecondary { get; set; }
    }
}