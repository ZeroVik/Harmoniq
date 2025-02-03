using Harmoniq.Models;
using Microsoft.EntityFrameworkCore;

namespace Harmoniq.Repositories.PlaylistSongRepositories
{
    public class PlaylistSongRepository : IPlaylistSongRepository
    {
        private readonly ApplicationDbContext _context;

        public PlaylistSongRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddSongToPlaylistAsync(PlaylistSong playlistSong)
        {
            await _context.PlaylistSongs.AddAsync(playlistSong);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveSongFromPlaylistAsync(int playlistId, int songId)
        {
            var playlistSong = await _context.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (playlistSong != null)
            {
                _context.PlaylistSongs.Remove(playlistSong);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Song>> GetSongsByPlaylistIdAsync(int playlistId)
        {
            return await _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .Select(ps => ps.Song)
                .ToListAsync();
        }
    }
}

