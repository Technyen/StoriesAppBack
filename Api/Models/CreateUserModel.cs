using Api.Enums;

namespace Api.Models
{
    public class RegisterUserModel
    {
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}
