using Api.Entities;
using Api.Models;
using AutoMapper;

namespace Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<RegisterUserModel, User>();
            CreateMap<LoginUserModel, User>();
        }
    }
}
