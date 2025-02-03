namespace Harmoniq.Dtos.AuthDtos
{
    public class AuthDto
    {
        public string Username { get; set; }
        public string Email { get; set; } // Only for registration
        public string FirstName { get; set; } // Only for registration
        public string LastName { get; set; } // Only for registration
        public string Password { get; set; }
    }
}
