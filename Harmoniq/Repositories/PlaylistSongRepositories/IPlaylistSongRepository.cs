using Harmoniq.Models;

namespace Harmoniq.Repositories.PlaylistSongRepositories
{
    public interface IPlaylistSongRepository
    {
        Task AddSongToPlaylistAsync(PlaylistSong playlistSong);
        Task RemoveSongFromPlaylistAsync(int playlistId, int songId);
        Task<List<Song>> GetSongsByPlaylistIdAsync(int playlistId);
    }
}
