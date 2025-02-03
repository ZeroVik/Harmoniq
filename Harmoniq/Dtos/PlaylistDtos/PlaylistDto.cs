namespace Harmoniq.Dtos.PlaylistDtos
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
