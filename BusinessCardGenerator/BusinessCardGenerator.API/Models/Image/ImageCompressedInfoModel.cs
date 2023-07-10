using ImageClass = BusinessCardGenerator.API.Data.Image;

namespace BusinessCardGenerator.API.Models.Image
{
    public class ImageCompressedInfoModel
    {
        public ImageCompressedInfoModel(ImageClass image, byte[] file)
        {
            Id = image.Id;
            FileName = image.FileName;
            Length = image.Length;
            File = file;
        }

        public Guid Id { get; set; }

        public string FileName { get; set; }

        public long Length { get; set; }

        public byte[] File { get; set; }
    }
}
