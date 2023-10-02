using Api.Enums;

namespace Api.Entities
{
    public class User
    {
        public string Type = nameof(User);
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserType UserType { get; set; }
    }
}
