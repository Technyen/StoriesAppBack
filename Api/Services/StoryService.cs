using Api.Entities;
using Api.Enums;

namespace Api.Services
{
    public class StoryService
    {
        private readonly ICosmosService _cosmosService;
        private readonly IStorageService _blobStorageService;
        private readonly string _imageContainerName;

        public StoryService(ICosmosService cosmosService, IStorageService blobStorageService)
        {
            _cosmosService = cosmosService;
            _blobStorageService = blobStorageService;
            _imageContainerName = "images";
        }

        public async Task<CreateResult> CreateStoryAsync(Story story, IFormFile formFile)
        {
            var storyFound = await _cosmosService.FindItemAsync<Story>(nameof(Story.Title), story.Title);
            if (storyFound == null)
            {
                story.Id = Guid.NewGuid().ToString();
                var extension = formFile.ContentType.Split("/").Last();
                story.ImageUrl = await _blobStorageService.UploadAsync(story.Id + "." + extension, formFile.OpenReadStream(), _imageContainerName);
                await _cosmosService.CreateItemAsync(story);

                return CreateResult.Success;
            }
            else
            {
                return CreateResult.Duplicate;
            }
        }

        public async Task<List<Story>> GetStoriesAsync()
        {
            var stories = await _cosmosService.GetItemsAsync<Story>();
            return stories;
        }

        public async Task<Story?> GetStoryAsync(string storyId)
        {
            var storyFound = await _cosmosService.FindItemAsync<Story>(nameof(Story.Id), storyId);
            return storyFound;
        }


        public async Task<EditResult> EditStoryAsync(Story story, IFormFile formFile)
        {
            var storyFound = await _cosmosService.FindItemAsync<Story>(nameof(Story.Id), story.Id);

            if (storyFound != null)
            {
                if (formFile != null)
                {
                    var oldExtension = storyFound.ImageUrl.Split("/").Last();
                    await _blobStorageService.DeleteAsync(oldExtension, _imageContainerName);
                    var newExtension = formFile.ContentType.Split("/").Last();
                    story.ImageUrl = await _blobStorageService.UploadAsync(story.Id + "." + newExtension, formFile.OpenReadStream(), _imageContainerName);
                }
                await _cosmosService.UpdateItemAsync(story, story.Id);
                return EditResult.Success;
            }
            else
            {
                return EditResult.NotFound;
            }
        }

        public async Task<DeleteResult> DeleteStoryAsync(string storyId)
        {
            var blobItem = await _blobStorageService.GetBlobAsync(storyId, _imageContainerName);
            await _blobStorageService.DeleteAsync(blobItem.Name, _imageContainerName);
            var response = await _cosmosService.DeleteItemAsync<Story>(storyId, nameof(Story));

            if (response != null)
            {
                return DeleteResult.Success;
            }
            else
            {
                return DeleteResult.NotFound;
            }
        }

    }
}
