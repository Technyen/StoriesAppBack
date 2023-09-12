using Api.Models;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
namespace Api.Services
{
    public interface IStorageService
    {
        Task UploadAsync(string fileName, Stream file);
        Task DeleteAsync(string blobName);
       Task<BlobItem?> GetBlobAsync(string blobName);


    }
}
