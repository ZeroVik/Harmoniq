namespace Harmoniq.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // Additional album details
        public DateTime ReleaseDate { get; set; }
        public string AlbumArtUrl { get; set; }

        // Foreign key and navigation property for the artist
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        // Navigation property for the album's songs
        public ICollection<Song> Songs { get; set; }
    }

}
