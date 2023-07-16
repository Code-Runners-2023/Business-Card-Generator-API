using ImageClass = BusinessCardGenerator.API.Data.Image;

namespace BusinessCardGenerator.API.Models.Image
{
    public class ImageCompressedInfoModel
    {
        public ImageCompressedInfoModel(ImageClass image, string baseLink)
        {
            Id = image.Id;
            Link = $"{baseLink}{Id}{image.FileExtension}";
        }

        public Guid Id { get; set; }

        public string Link { get; set; }
    }
}
