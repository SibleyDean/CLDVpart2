using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;


using System.Collections.Generic;

namespace eventEasefour
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["DefaultEndpointsProtocol=https;AccountName=cldvimages;AccountKey=wPONwrbrz46hhPkTML3fHxpharQ1p97D/swEuHciyeMwfPZIKzZQMHqJyqZUyXSyL0Bsd2xr+y8W+ASt5flylQ==;EndpointSuffix=core.windows.net"];
            var containerName = configuration["imagesds"];
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = _containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString();
        }

        public async Task DeleteFileAsync(string blobUrl)
        {
            var blobName = Path.GetFileName(new Uri(blobUrl).LocalPath);
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
