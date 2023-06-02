namespace BooksForYou.Services.AzureServices
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class AzureImageService : IAzureImageService
    {
        private readonly AzureOptConfig _azureOptions;

        public AzureImageService(IOptions<AzureOptConfig> azureOptions)
        {
            _azureOptions = azureOptions.Value;
        }

        public async Task<Uri> UploadImageToAzureAsync(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);

            using MemoryStream fileUploadStream = new MemoryStream();
            file.CopyTo(fileUploadStream);

            fileUploadStream.Position = 0;
            BlobContainerClient blobContainerClient = new BlobContainerClient(
                 _azureOptions.ConnectionString,
                 _azureOptions.Container);

            var uniqueName = Guid.NewGuid().ToString() + fileExtension;
            BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueName);

            await blobClient.UploadAsync(fileUploadStream, new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            }, cancellationToken: default);

            var uri = blobClient.Uri;

            return uri;
        }

        public async Task DeleteImageFromAzureAsync(string uriString)
        {
            Uri uri = new Uri(uriString);

            CloudBlockBlob blobName = new CloudBlockBlob(uri);

            string fileName = blobName.Name;
            try
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(
                 _azureOptions.ConnectionString,
                 _azureOptions.Container);

                if (await blobContainerClient.ExistsAsync())
                {
                    BlobClient blob = blobContainerClient.GetBlobClient(fileName);

                    if (await blob.ExistsAsync())
                    {
                        await blob.DeleteAsync();
                    }
                }
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
