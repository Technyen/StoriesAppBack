using Api.Entities;
using Api.Enums;

namespace Api.Services
{
    public class StoryService
    {
        private readonly ICosmosService _serviceCosmos;

        public StoryService(ICosmosService serviceCosmos)
        {
            _serviceCosmos = serviceCosmos;
        }

        public async Task<CreateResult> CreateStory(Story story)
        {
            var storyFound = await _serviceCosmos.FindItemAsync<Story>(story.Title, nameof(Story.Title));
            if (storyFound == null)
            {
                story.Id = Guid.NewGuid().ToString();
                await _serviceCosmos.CreateItemAsync(story);
                return CreateResult.Success;
            }
            else
            {
                return CreateResult.Duplicate;
            }
        }

        public async Task<List<Story>> GetStories()
        {
            var stories = await _serviceCosmos.GetItemsAsync<Story>();
            return stories;
        }
    }
}
