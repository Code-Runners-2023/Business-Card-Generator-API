namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IAzureCloudService
    {
        byte[] GetFileFromCloud(Guid id, string fileExtension);

        void UploadFileInCloud(Guid id, IFormFile file);

        void UpdateFileInCloud(Guid id, string fileExtension, IFormFile file);

        byte[] DeleteFileFromCloud(Guid id, string fileExtension);
    }
}
