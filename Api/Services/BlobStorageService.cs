using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        public async Task<BlobItem?> GetBlobAsync(string blobName)
        {
            var container = _blobServiceClient.GetBlobContainerClient("images");
            var asyncPageable = container.GetBlobsAsync(prefix: blobName);
            return await asyncPageable.FirstOrDefaultAsync();
        }
		
		public async Task DeleteAsync(string blobName)
		{
            var container = _blobServiceClient.GetBlobContainerClient("images");
            await container.DeleteBlobAsync(blobName);





        }

    }
}
