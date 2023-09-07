using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

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
		

	}
}
