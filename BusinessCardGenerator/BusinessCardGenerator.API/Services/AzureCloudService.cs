using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BusinessCardGenerator.API.Services.Interfaces;

namespace BusinessCardGenerator.API.Services
{
    public class AzureCloudService : IAzureCloudService
    {
        private readonly string blobUrl;
        private readonly string resourceGroup;
        private readonly string account;
        private readonly string container;
        private readonly string connectionString;

        public AzureCloudService(IConfiguration configuration)
        {
            var azureSettings = configuration.GetSection("AzureSettings") ??
                                throw new InvalidOperationException("'AzureSettings' not found.");

            blobUrl = azureSettings["BlobUrl"] ?? throw new InvalidOperationException("Azure BlobUrl not found!");
            
            resourceGroup = azureSettings["ResourceGroup"] ?? throw new InvalidOperationException("Azure ResourceGroup not found!");
            
            account = azureSettings["Account"] ?? throw new InvalidOperationException("Azure Account not found!");
            
            container = azureSettings["Container"] ?? throw new InvalidOperationException("Azure Container not found!");
            
            connectionString = configuration.GetConnectionString("AzureConnection") ??
                               throw new InvalidOperationException("Azure Connection String not found!");
        }

        public byte[] GetFileFromCloud(Guid id, string fileExtension)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);

            string fileName = $"{id}{fileExtension}";
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            BlobProperties blobProperties = blobClient.GetProperties();
            byte[] fileBytes = new byte[blobProperties.ContentLength];

            using MemoryStream fileDownloadStream = new MemoryStream(fileBytes);
            blobClient.DownloadTo(fileDownloadStream);

            return fileBytes;
        }

        public void UploadFileInCloud(Guid id, IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);

            using MemoryStream fileUploadStream = new MemoryStream();
            
            file.CopyTo(fileUploadStream);
            fileUploadStream.Position = 0;
            
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, container);

            string fileName = $"{id}{fileExtension}";
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            blobClient.Upload(fileUploadStream, new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/bitmap"
                }
            }, cancellationToken: default);
        }

        public void UpdateFileInCloud(Guid id, string fileExtension, IFormFile file)
        {
            using MemoryStream fileUploadStream = new MemoryStream();

            file.CopyTo(fileUploadStream);
            fileUploadStream.Position = 0;

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, container);

            string fileName = $"{id}{fileExtension}";
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            blobClient.Upload(fileUploadStream, true);
        }

        public byte[] DeleteFileFromCloud(Guid id, string fileExtension)
        {
            return null;
        }
    }
}
