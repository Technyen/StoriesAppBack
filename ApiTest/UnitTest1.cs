using Api.Entities;
using Api.Enums;
using Api.Services;
using Moq;

namespace ApiTest
{
    public class UnitTest1
    {
        public readonly Mock<ICosmosService> _cosmosServiceMock = new();

        [Fact]
        public async void TestGetStories()
        {
            // Arrange
            var stories = new List<Story>();
            _cosmosServiceMock.Setup( x => x.GetItemsAsync<Story>()).ReturnsAsync(stories);
            StoryService storyService = new(_cosmosServiceMock.Object);
            
            // Act
            var result =await storyService.GetStories();

            // Assert
            Assert.Equal(stories, result);
        }

        [Fact]
        public async void TestCreateStory()
        {
            // Arrange
            var story = new Story();
            _cosmosServiceMock.Setup(x => x.FindItemAsync<Story>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(story);
            StoryService storyService = new(_cosmosServiceMock.Object);

            // Act
            var result = await storyService.CreateStory(story);

            // Assert
            Assert.Equal(CreateResult.Duplicate, result);    

        }

        [Fact]
        public async void TestCreateStory2()
        {
            // Arrange
            var story = new Story();
            _cosmosServiceMock.Setup(x => x.FindItemAsync<Story>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(Story));
            StoryService storyService = new(_cosmosServiceMock.Object);

            // Act
            var result = await storyService.CreateStory(story);
           

            // Assert
            Assert.Equal(CreateResult.Success, result);
          

        }
    }
}