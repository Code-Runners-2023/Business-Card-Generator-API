using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services.Interfaces;

namespace BusinessCardGenerator.API.Services
{
    public class AzureCloudService : IAzureCloudService
    {
        private readonly ApplicationSettings settings;

        public AzureCloudService(IConfiguration config)
        {
            settings = new ApplicationSettings(config);
        }

        public void UploadFileInCloud(Guid id, IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);

            using MemoryStream fileUploadStream = new MemoryStream();
            
            file.CopyTo(fileUploadStream);
            fileUploadStream.Position = 0;
            
            BlobContainerClient blobContainerClient = new BlobContainerClient(settings.AzureConnectionString, settings.AzureContainer);

            string fileName = $"{id}{fileExtension}";
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            blobClient.Upload(fileUploadStream, new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = $"image/{fileExtension.Replace(".", "")}"
                }
            }, cancellationToken: default);
        }

        public void UpdateFileInCloud(Guid id, string fileExtension, IFormFile file)
        {
            string newFileExtension = Path.GetExtension(file.FileName);

            if (fileExtension != newFileExtension)
                DeleteFileFromCloud(id, fileExtension);

            UploadFileInCloud(id, file);
        }

        public void DeleteFileFromCloud(Guid id, string fileExtension)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(settings.AzureConnectionString, settings.AzureContainer);

            string fileName = $"{id}{fileExtension}";
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            blobClient.DeleteIfExists();
        }
    }
}
