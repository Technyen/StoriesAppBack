using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BlobStorageController
    {
        [ApiController]
        [Route("[controller]")]
        public class BlobController : ControllerBase
        {
            private readonly BlobServiceClient _blobServiceClient;

            public BlobController(BlobServiceClient blobServiceClient)
            {
                _blobServiceClient = blobServiceClient;
            }

            [HttpGet]
            public async Task<IEnumerable<string>> Get()
            {
                BlobContainerClient containerClient =
                    _blobServiceClient.GetBlobContainerClient("demo");
                var results = new List<string>();

                await foreach (BlobItem blob in containerClient.GetBlobsAsync())
                {
                    results.Add(blob.Name);
                }

                return results.ToArray();
            }
        }
    }
}
