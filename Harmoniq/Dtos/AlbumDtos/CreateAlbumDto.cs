namespace Harmoniq.Dtos.AlbumDtos
{
    public class CreateAlbumDto
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
        public IFormFile AlbumArt { get; set; } // For album cover image upload
    }
}
