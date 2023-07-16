using BcardClass = BusinessCardGenerator.API.Data.BusinessCard;
using ImageClass = BusinessCardGenerator.API.Data.Image;

namespace BusinessCardGenerator.API.Models.Image
{
    public class ImageAzureFileModel
    {
        public ImageAzureFileModel(ImageClass image)
        {
            Id = image.Id;
            FileExtension = image.FileExtension;
        }

        public ImageAzureFileModel(BcardClass bcard)
        {
            Id = bcard.Id;
            FileExtension = bcard.LogoFileExtension;
        }

        public Guid Id { get; private set; }

        public string FileExtension { get; private set; }
    }
}
