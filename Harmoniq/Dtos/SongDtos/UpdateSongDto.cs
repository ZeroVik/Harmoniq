namespace Harmoniq.Dtos.SongDtos
{
    public class UpdateSongDto
    {
        public string Title { get; set; }
        public int? AlbumId { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? TrackNumber { get; set; }
    }
}
