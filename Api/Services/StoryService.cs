using Api.Entities;
using Api.Enums;


namespace Api.Services
{
    public class StoryService
    {
        private readonly ICosmosService _cosmosService;
        private readonly IStorageService _blobStorageService;

        public StoryService(ICosmosService cosmosService, IStorageService blobStorageService )
        {
            _cosmosService = cosmosService;
            _blobStorageService = blobStorageService;
        }

        public async Task<CreateResult> CreateStoryAsync(Story story, IFormFile formFile)
        {
            var storyFound = await _cosmosService.FindItemAsync<Story>(nameof(Story.Title), story.Title);
            if (storyFound == null)
            {
                story.Id = Guid.NewGuid().ToString() ;
                await _cosmosService.CreateItemAsync(story);
                string blobName = story.Id + "." + formFile.ContentType.Split("/").Last();
                await _blobStorageService.UploadAsync(blobName , formFile.OpenReadStream() );

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
                    await _blobStorageService.DeleteAsync(formFile.Name);
                    await _blobStorageService.UploadAsync(story.Id, formFile.OpenReadStream());
                }
                storyFound.Title = story.Title;
                storyFound.Category = story.Category;
                storyFound.AgeSuggested = story.AgeSuggested; 
                storyFound.Description = story.Description;
                await _cosmosService.UpdateItemAsync(storyFound, storyFound.Id);
                return EditResult.Success;
            }
            else
            {
                return EditResult.NotFound;
            }
        }

        public async Task<DeleteResult> DeleteStoryAsync(string storyId)
        {
            var blobItem = await _blobStorageService.GetBlobAsync(storyId);
            await _blobStorageService.DeleteAsync(blobItem.Name);
            var response = await _cosmosService.DeleteItemAsync<Story>(storyId , nameof(Story));
           
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
