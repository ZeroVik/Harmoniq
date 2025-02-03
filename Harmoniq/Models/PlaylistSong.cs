namespace Harmoniq.Models
{
    public class PlaylistSong
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        // Optional: store the order of the song in the playlist
        public int Order { get; set; }
    }

}
