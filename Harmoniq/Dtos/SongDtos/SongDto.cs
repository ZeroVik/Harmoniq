namespace Harmoniq.Dtos.SongDtos
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AlbumId { get; set; }
        public TimeSpan Duration { get; set; }
        public int TrackNumber { get; set; }
        public string FilePath { get; set; }
    }
}
