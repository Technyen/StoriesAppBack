using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Api.Enums;
using Api.Entities;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserService _serviceUsers;
        private readonly IMapper _mapper;
        public UsersController( ILogger<UsersController> logger, UserService serviceUser, IMapper mapper)
        {
            _logger = logger;
            _serviceUsers = serviceUser;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserModel registerUserModel)
        {
            var user = _mapper.Map<User>(registerUserModel);

            var result =await _serviceUsers.RegisterUser(user);
            if (result == RegisterResult.Success)
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserModel loginUserModel)
        {
            var user = _mapper.Map<User>(loginUserModel);
            var result= await _serviceUsers.LoginUser(user);
            if (result == LoginResult.Success)
            {
                return Ok();
            }
            else if (result == LoginResult.NotFound)
            {
                return NotFound();
            }
            else
            {
                return Unauthorized();    
            }
        }
    }
}
