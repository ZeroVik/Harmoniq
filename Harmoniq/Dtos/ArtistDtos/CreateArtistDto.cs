namespace Harmoniq.Dtos.ArtistDtos
{
    public class CreateArtistDto
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public IFormFile ProfileImage { get; set; } // For artist profile image upload
    }
}
