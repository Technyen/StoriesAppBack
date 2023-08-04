using Api.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Api.Services;
using Api.Controllers;
using Api.Enums;
using Api.Entities;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly StoryService _serviceStories;
        private readonly IMapper _mapper;



        public StoriesController(ILogger<StoriesController> logger, StoryService serviceStory,IMapper mapper )
        {
            _logger = logger;
            _serviceStories = serviceStory;
            _mapper = mapper;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> Create(CreateStoryModel createStoryModel)
        {
            var story =_mapper.Map<Story>(createStoryModel);
            var result = await _serviceStories.CreateStory(story);
            if (result == CreateResult.Success)
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }

        [HttpGet("GetAll")]
        public async Task<List<Story>> GetStories()
        {
            var result = await _serviceStories.GetStories();
            return result;
        }


    }
}
