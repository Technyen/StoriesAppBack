using Api.Models;
using Microsoft.Extensions.Azure;
namespace Api.Services
{
    public interface IStorageService
    {
        Task UploadAsync(string fileName, Stream file);
        Task DeleteAsync(string blobName);
        Task GetBlobAsync();


    }
}
