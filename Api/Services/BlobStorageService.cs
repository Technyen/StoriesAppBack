using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Api.Services
{
	public class StorageService : IStorageService
	{
        private readonly BlobServiceClient _blobServiceClient;

        public StorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
		
		public async Task UploadAsync(string fileName, Stream file)
		{
            var container = _blobServiceClient.GetBlobContainerClient("images");
			await container.UploadBlobAsync(fileName, file);
        }

        public async Task GetBlobAsync()
        {
            var container = _blobServiceClient.GetBlobContainerClient("images");
            await foreach (BlobItem blobItem in container.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }   
        }
		
		public async Task DeleteAsync(string blobName)
		{
            var container = _blobServiceClient.GetBlobContainerClient("images");
            await container.DeleteBlobAsync(blobName);
            

        }

    }
}
