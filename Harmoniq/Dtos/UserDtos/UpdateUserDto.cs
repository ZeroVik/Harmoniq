namespace Harmoniq.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public string Username { get; set; }       // Optional: allow username change
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
