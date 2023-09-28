using Api.Entities;
using Api.Enums;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ApiTest
{
    public class UnitTest1
    {
        public readonly Mock<IRepositoryService> _cosmosServiceMock = new();
        public readonly Mock<IStorageService> _storageServiceMock = new();

        [Fact]
        public async void TestGetStories()
        {
            // Arrange
            var stories = new List<Story>();
            _cosmosServiceMock.Setup( x => x.GetItemsAsync<Story>()).ReturnsAsync(stories);
            StoryService storyService = new(_cosmosServiceMock.Object, _storageServiceMock.Object);
            
            // Act
            var result =await storyService.GetStoriesAsync();

            // Assert
            Assert.Equal(stories, result);
        }

        [Fact]
        public async void CreateStoryAsyncWithExistentStoryErrorsDuplicate()
        {
            // Arrange
            Mock<Stream> fileMock = new Mock<Stream>();
            var story = new Story();
            _cosmosServiceMock.Setup(x => x.FindItemAsync<Story>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(story);
            _storageServiceMock.Setup(x => x.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(),"images"));
            StoryService storyService = new(_cosmosServiceMock.Object, _storageServiceMock.Object);

            // Act
            var result = await storyService.CreateStoryAsync(story, (IFormFile)fileMock.Object);

            // Assert
            Assert.Equal(CreateResult.Duplicate, result);    

        }

        [Fact]
        public async void CreateStoryAsyncWithNonExistentStorySucceeds()
        {
            // Arrange
            var file = new Mock<Stream>();
            var story = new Story();
            _cosmosServiceMock.Setup(x => x.FindItemAsync<Story>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(Story));
            _storageServiceMock.Setup(x => x.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(), "images"));
            StoryService storyService = new(_cosmosServiceMock.Object, _storageServiceMock.Object);

            // Act
            var result = await storyService.CreateStoryAsync(story, (IFormFile)file.Object);
           

            // Assert
            Assert.Equal(CreateResult.Success, result);
          

        }
    }
}