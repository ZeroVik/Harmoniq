namespace Harmoniq.Dtos.AlbumDtos
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AlbumArtUrl { get; set; }
        public int ArtistId { get; set; }
    }
}
