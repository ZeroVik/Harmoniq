namespace Harmoniq.Dtos.ArtistDtos
{
    public class UpdateArtistDto
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
