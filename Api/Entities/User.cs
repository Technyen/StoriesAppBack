using Api.Enums;

namespace Api.Entities
{
    public class User
    {
        public string Type = nameof(User);
        public int age { get; set; }    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}
