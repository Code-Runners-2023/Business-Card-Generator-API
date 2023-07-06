namespace BusinessCardGenerator.API.Models.Image
{
    public class ImageCompressedInfoModel
    {
        public ImageCompressedInfoModel(Guid id, string file)
        {
            Id = id;
            File = file;
        }

        public Guid Id { get; set; }

        public string File { get; set; }
    }
}
