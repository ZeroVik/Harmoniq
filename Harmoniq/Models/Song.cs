namespace Harmoniq.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // Foreign key to the album and corresponding navigation property
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public TimeSpan Duration { get; set; }
        public int TrackNumber { get; set; }

        // File path or URL to the audio file
        public string FilePath { get; set; }

        // Navigation property for many-to-many relationship with Playlist via PlaylistSong
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
    }

}
