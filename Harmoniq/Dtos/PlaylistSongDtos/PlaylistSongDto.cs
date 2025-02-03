using Harmoniq.Models;

namespace Harmoniq.Dtos.PlaylistSongDtos
{
    public class PlaylistSongDto
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
        public int Order { get; set; } // Optional ordering
    }
}
