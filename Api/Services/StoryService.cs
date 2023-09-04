using Api.Entities;
using Api.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;

namespace Api.Services
{
    public class StoryService
    {
        private readonly ICosmosService _cosmosService;

        public StoryService(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        public async Task<CreateResult> CreateStoryAsync(Story story)
        {
            var storyFound = await _cosmosService.FindItemAsync<Story>(nameof(Story.Id), story.Id);
            if (storyFound == null)
            {
                story.Id = Guid.NewGuid().ToString();
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
