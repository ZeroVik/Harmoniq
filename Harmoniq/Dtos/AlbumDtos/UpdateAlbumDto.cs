namespace Harmoniq.Dtos.AlbumDtos
{
    public class UpdateAlbumDto
    {
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public IFormFile? AlbumArt { get; set; }
    }
}
