using BusinessCardGenerator.API.Services.Interfaces;

namespace BusinessCardGenerator.API.Services
{
    public class AzureCloudService : IAzureCloudService
    {
        public byte[] GetFileFromCloud(Guid id)
        {
            return null;
        }

        public void SaveFileInCloud(Guid id, IFormFile file)
        {
            return;
        }

        public void UpdateFileInCloud(Guid id, IFormFile file)
        {
            return;
        }

        public byte[] DeleteFileFromCloud(Guid id)
        {
            return null;
        }
    }
}
