using Api.Entities;
using Api.Models;
using AutoMapper;

namespace Api.Profiles
{
    public class StoryProfile : Profile
    {
        public StoryProfile() 
        {
            CreateMap<CreateStoryModel, Story>();
            CreateMap<EditStoryModel, Story>();
        }
    }
}
