namespace Harmoniq.Dtos.SongDtos
{
    public class CreateSongDto
    {
        public string Title { get; set; }
        public int AlbumId { get; set; }
        public TimeSpan Duration { get; set; }
        public int TrackNumber { get; set; }
        public IFormFile SongFile { get; set; } // Song file upload
    }
}
