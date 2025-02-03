namespace Harmoniq.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Additional details for the artist
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }

        // Navigation property for related albums
        public ICollection<Album> Albums { get; set; }
    }

}
