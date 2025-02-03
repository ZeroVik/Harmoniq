namespace Harmoniq.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? LastUsernameChange { get; set; }

        // Additional profile details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Subscription Subscription { get; set; }

        // Navigation property for the user's playlists
        public ICollection<Playlist> Playlists { get; set; }
    }

}
