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
        private readonly StoryService _storyService;
        private readonly IMapper _mapper;



        public StoriesController(ILogger<StoriesController> logger, StoryService storyService, IMapper mapper)
        {
            _logger = logger;
            _storyService = storyService;
            _mapper = mapper;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateAsync(CreateStoryModel createStoryModel)
        {
            
            var story = _mapper.Map<Story>(createStoryModel);
            var result = await _storyService.CreateStoryAsync(story);
            if (result == CreateResult.Success)
            {
                return Ok(story.Id) ;
            }
            else
            {
                return Conflict();
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Story>>> GetStories()
        {
            try
            {
                var result = await _storyService.GetStoriesAsync();
                return result;
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{Title}")]
        public async Task<ActionResult<Story>> GetStoryAsync(string title)
        {
            try
            {
                var result = await _storyService.GetStoryAsync(title);
                return result;
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpPut("EditStory")]
        public async Task<ActionResult> EditStoryAsync(EditStoryModel editStoryModel)
        {
            var story = _mapper.Map<Story>(editStoryModel);
            var result = await _storyService.EditStoryAsync(story);
            if (result == EditResult.Success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{StoryId}")]
        public async Task<ActionResult> DeleteStoryAsync(string storyId)
        {
            try
            {
                var response = await _storyService.DeleteStoryAsync(storyId);
                if (response== DeleteResult.Success)
                {
                    return Ok();

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

               return BadRequest(ex.Message);
            }
        }


    }
}
