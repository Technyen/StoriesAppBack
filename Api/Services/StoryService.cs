using Api.Entities;
using Api.Enums;

namespace Api.Services
{
    public class StoryService
    {
        private readonly ICosmosService _cosmosService;

        public StoryService(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        public async Task<CreateResult> CreateStory(Story story)
        {
            var storyFound = await _cosmosService.FindItemAsync<Story>(nameof(Story.Title), story.Title);
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

        public async Task<List<Story>> GetStories()
        {
            var stories = await _cosmosService.GetItemsAsync<Story>();
            return stories;
        }

        public async Task<EditResult> EditStory(Story story)
        {
            var storyFound = await _cosmosService.FindItemAsync<Story>(nameof(Story.Title), story.Title);
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
    }
}
