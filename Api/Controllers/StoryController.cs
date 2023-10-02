using Api.Entities;
using Api.Enums;
using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("create")]
        public async Task<ActionResult> CreateAsync([FromForm] CreateStoryModel createStoryModel)
        {
            try
            {
                var story = _mapper.Map<Story>(createStoryModel);
                var result = await _storyService.CreateStoryAsync(story, createStoryModel.FormFile);
                if (result == CreateResult.Success)
                {
                    return Ok(story.Id);
                }
                else
                {
                    return Conflict();
                }
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [HttpGet("getAll")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Story>?> GetStoryAsync(string id)
        {
            try
            {
                var result = await _storyService.GetStoryAsync(id);
                if (result != null) 
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("editStory")]
        public async Task<ActionResult> EditStoryAsync([FromForm] EditStoryModel editStoryModel)
        {
            try
            {
                var story = _mapper.Map<Story>(editStoryModel);
                var result = await _storyService.EditStoryAsync(story, editStoryModel.FormFile);
                if (result == EditResult.Success)
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

                return Problem(ex.Message);
            }
        }

        [HttpDelete("{storyId}")]
        public async Task<ActionResult> DeleteStoryAsync(string storyId)
        {
            try
            {
                var response = await _storyService.DeleteStoryAsync(storyId);

                if (response == DeleteResult.Success)
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
