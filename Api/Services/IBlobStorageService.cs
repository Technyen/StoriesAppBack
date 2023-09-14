using Azure.Storage.Blobs.Models;
namespace Api.Services
{
    public interface IStorageService
    {
        Task<string> UploadAsync(string blobName, Stream file, string containerName);
        Task<BlobItem?> GetBlobAsync(string blobName, string containerName);
        Task DeleteAsync(string blobName, string containerName);
    }
}
