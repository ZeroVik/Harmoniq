namespace Harmoniq.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Additional details for the playlist
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key and navigation property for the user who created the playlist
        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation property for the many-to-many relationship with songs
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
    }

}
