using ImageClass = BusinessCardGenerator.API.Data.Image;

namespace BusinessCardGenerator.API.Models.Image
{
    public class ImageCompressedInfoModel
    {
        public ImageCompressedInfoModel(ImageClass image, byte[] file)
        {
            Id = image.Id;
            FileExtension = image.FileExtension;
            File = file;
        }

        public Guid Id { get; set; }

        public string FileExtension { get; set; }

        public byte[] File { get; set; }
    }
}
