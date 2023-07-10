namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IAzureCloudService
    {
        byte[] GetFileFromCloud(Guid id);

        void SaveFileInCloud(Guid id, IFormFile file);

        void UpdateFileInCloud(Guid id, IFormFile file);

        byte[] DeleteFileFromCloud(Guid id);
    }
}
