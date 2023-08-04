using Microsoft.Identity.Client;

namespace Api.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Email { get; }
        public string Password { get; }
        public NotFoundException()
        {

        }
        public NotFoundException(string message) : base(message)
        {

        }
        public NotFoundException(string message, Exception inner)
       : base(message, inner) { }

        public NotFoundException(string email, string password) : this(email)
        {
            Email = email;
            Password = password;
        }
    }
}
