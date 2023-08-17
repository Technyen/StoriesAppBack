using Api.Entities;
using Api.Enums;

namespace Api.Services
{
    public class UserService
    {
        private readonly ICosmosService _serviceCosmos;

        public UserService(ICosmosService serviceCosmos)
        {
            _serviceCosmos = serviceCosmos;
        }

        public async Task<RegisterResult> RegisterUser(User user)
        {
            var userFound = await _serviceCosmos.FindItemAsync<User>(nameof(User.Email), user.Email);
            if (userFound == null)
            {
                user.Id = Guid.NewGuid().ToString();
                await _serviceCosmos.CreateItemAsync(user);
                return RegisterResult.Success;
            }
            else
            {
                return RegisterResult.Duplicate;
            }
        }

        public async Task<LoginResult> LoginUser(User user)
        {
            var userFound = await _serviceCosmos.FindItemAsync<User>(nameof(User.Email), user.Email);
            if (userFound ==null)
            {
                return LoginResult.NotFound;

            }
            else if (user.Password == userFound.Password)
            {
                return LoginResult.Success;
            }
            else
            {
                return LoginResult.InvalidPassword;
            }
        }
    }
}
