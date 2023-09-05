//using Api.Models;
//using Api.Services;
//using Azure.Storage.Blobs;

//namespace Api.Repository
//{
//    public class AzureStorage : IAzureStorage
//	{
//        #region Dependency Injection / Constructor

//        private readonly BlobServiceClient _blobServiceClient;
//        private readonly string _storageContainerName;
//		private readonly ILogger<AzureStorage> _logger;

//		public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
//		{
//            _blobServiceClient = configuration.GetValue<string>("BlobConnectionString");
//			_storageContainerName = configuration.GetValue<string>("BlobContainerName");
//			_logger = logger;
//		}

//        #endregion
//        public async Task<BlobResponseDto> UploadAsync(IFormFile file)
//		{

//		}
//        public async Task<BlobDto> DownloadAsync(string blobFilename)
//		{

//		}
//        public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
//		{

//		}
//		public async Task<List<BlobDto>> ListAsync() 
//		{ 
//		}

//    }
//}
