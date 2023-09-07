using Api.Entities;
using Api.Enums;
using Api.Models;
using Azure.Storage.Blobs;

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
                story.Id = Guid.NewGuid().ToString();
                await _cosmosService.CreateItemAsync(story);
                await _blobStorageService.UploadAsync(formFile.FileName, formFile.OpenReadStream() );

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


        public async Task<EditResult> EditStoryAsync(Story story)
        {
            var storyFound = await _cosmosService.FindItemAsync<Story>(nameof(Story.Id), story.Id);
            if(storyFound != null)
            {
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
            var response = await _cosmosService.DeleteItemAsync<Story>(storyId , nameof(Story));
            if(response != null)
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
