using Api.Entities;
using Api.Enums;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace ApiTest
{
    public class StoryServiceTest
    {
        public readonly Mock<IRepositoryService> _cosmosServiceMock = new();
        public readonly Mock<IStorageService> _storageServiceMock = new();

        [Fact]
        public async void GetStoriesAsynReturnStories()
        {
            var stories = new List<Story>();
            _cosmosServiceMock.Setup( x => x.GetItemsAsync<Story>()).ReturnsAsync(stories);
            StoryService storyService = new(_cosmosServiceMock.Object, _storageServiceMock.Object);
            
            var result =await storyService.GetStoriesAsync();

            Assert.Equal(stories, result);
        }

        [Fact]
        public async void CreateStoryAsyncWithExistentStoryErrorsDuplicate()
        {
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            var story = new Story();
            _cosmosServiceMock.Setup(x => x.FindItemAsync<Story>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(story);
            _storageServiceMock.Setup(x => x.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(),"images"));
            StoryService storyService = new(_cosmosServiceMock.Object, _storageServiceMock.Object);

            var result = await storyService.CreateStoryAsync(story, (IFormFile)fileMock.Object);

            Assert.Equal(CreateResult.Duplicate, result);    
        }

        [Fact]
        public async void CreateStoryAsyncWithNonExistentStorySucceeds()
        {
            var fileMock = new Mock<IFormFile>();
            var story = new Story();
            fileMock.Setup(x => x.ContentType).Returns("application/jpg");
            _cosmosServiceMock.Setup(x => x.FindItemAsync<Story>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(Story));
            _storageServiceMock.Setup(x => x.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(), "images"));
            StoryService storyService = new(_cosmosServiceMock.Object, _storageServiceMock.Object);

            var result = await storyService.CreateStoryAsync(story, (IFormFile)fileMock.Object);

            Assert.Equal(CreateResult.Success, result);
        }
    }
}
