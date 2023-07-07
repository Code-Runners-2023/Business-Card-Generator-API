using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessCardGenerator.API.Models.BusinessCard
{
    public class BusinessCardInputModel
    {

        [StringLength(120, ErrorMessage = "Too long company name! Max length is 120 characters!")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "Too long address! Max length is 250 characters!")]
        public string Address { get; set; }

        [Url]
        public string Website { get; set; }

        public string HexColorCodeMain { get; set; }

        public string HexColorCodeSecondary { get; set; }

        public IFormFile LogoFile { get; set; }
    }
}
