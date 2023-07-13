using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BusinessCardClass = BusinessCardGenerator.API.Data.BusinessCard;

namespace BusinessCardGenerator.API.Models.BusinessCard
{
    public class BusinessCardCompressedInfoModel
    {
        public BusinessCardCompressedInfoModel(BusinessCardClass bcard, byte[] logoFile)
        {
            Id = bcard.Id;
            Name = bcard.Name;
            Address = bcard.Address;
            Website = bcard.Website;
            LogoFileExtension = bcard.LogoFileExtension;
            LogoFile = logoFile;
            HexColorCodeMain = bcard.HexColorCodeMain;
            HexColorCodeSecondary = bcard.HexColorCodeSecondary;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public string LogoFileExtension { get; set; }

        public byte[] LogoFile { get; set; }

        public string HexColorCodeMain { get; set; }

        public string HexColorCodeSecondary { get; set; }
    }
}
