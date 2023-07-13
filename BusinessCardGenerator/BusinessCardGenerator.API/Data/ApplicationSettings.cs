using System.Data.Common;

namespace BusinessCardGenerator.API.Data
{
    public class ApplicationSettings
    {
        public ApplicationSettings(IConfiguration config)
        {
            PostgresConnectionString = config.GetConnectionString("PostgresConnection") ??
                                       throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");

            AzureConnectionString = config.GetConnectionString("AzureConnection") ??
                                    throw new InvalidOperationException("Connection string 'AzureConnection' not found.");

            var jwtSettings = config.GetSection("JwtSettings") ??
                              throw new InvalidOperationException("'JwtSettings' not found.");

            JwtIssuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not found!");
            JwtAudience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not found!");
            JwtSecretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not found!");

            FrontendUrl = config.GetValue<string>("FrontendUrl") ??
                          throw new InvalidOperationException("FrontendUrl not found!");

            var azureSettings = config.GetSection("AzureSettings") ??
                                throw new InvalidOperationException("'AzureSettings' not found.");

            AzureBlobUrl = azureSettings["BlobUrl"] ?? throw new InvalidOperationException("Azure BlobUrl not found!");
            AzureResourceGroup = azureSettings["ResourceGroup"] ?? throw new InvalidOperationException("Azure ResourceGroup not found!");
            AzureAccount = azureSettings["Account"] ?? throw new InvalidOperationException("Azure Account not found!");
            AzureContainer = azureSettings["Container"] ?? throw new InvalidOperationException("Azure Container not found!");
        }

        public string PostgresConnectionString { get; private set; }

        public string JwtIssuer { get; private set; }

        public string JwtAudience {get; private set; }

        public string JwtSecretKey {get; private set; }

        public string FrontendUrl {get; private set; }

        public string AzureConnectionString { get; private set; }

        public string AzureBlobUrl {get; private set; }

        public string AzureResourceGroup {get; private set; }

        public string AzureAccount {get; private set; }

        public string AzureContainer {get; private set; }
    }
}
