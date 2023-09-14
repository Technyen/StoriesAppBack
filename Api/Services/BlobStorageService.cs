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
		
		public async Task<string> UploadAsync(string blobName, Stream file, string containerName)
		{
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            
			var response = await blobContainerClient.UploadBlobAsync(blobName, file);
            
            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<BlobItem?> GetBlobAsync(string blobName, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            var asyncPageable = container.GetBlobsAsync(prefix: blobName);
            return await asyncPageable.FirstOrDefaultAsync();
        }
		
		public async Task DeleteAsync(string blobName, string containerName)
		{
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.DeleteBlobAsync(blobName);
        }

    }
}
