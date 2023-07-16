namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IAzureCloudService
    {
        void UploadFileInCloud(Guid id, IFormFile file);

        void UpdateFileInCloud(Guid id, string fileExtension, IFormFile file);

        void DeleteFileFromCloud(Guid id, string fileExtension);
    }
}
