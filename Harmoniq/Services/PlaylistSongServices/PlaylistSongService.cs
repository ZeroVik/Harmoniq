using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.PlaylistSongDtos;
using Harmoniq.Dtos;
using Harmoniq.Models;
using Harmoniq.Repositories;
using Harmoniq.Repositories.PlaylistSongRepositories;
using Harmoniq.Repositories.SongRepositories;
using Harmoniq.Repositories.PlaylistRepositories;

namespace Harmoniq.Services
{
    public class PlaylistSongService
    {
        private readonly IPlaylistSongRepository _playlistSongRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;

        public PlaylistSongService(IPlaylistSongRepository playlistSongRepository,
                                   IPlaylistRepository playlistRepository,
                                   ISongRepository songRepository)
        {
            _playlistSongRepository = playlistSongRepository;
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
        }

        public async Task<bool> AddSongToPlaylistAsync(PlaylistSongDto dto)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(dto.PlaylistId);
            var song = await _songRepository.GetSongByIdAsync(dto.SongId);

            if (playlist == null || song == null)
                return false;

            var playlistSong = new PlaylistSong
            {
                PlaylistId = dto.PlaylistId,
                SongId = dto.SongId,
                Order = dto.Order
            };

            await _playlistSongRepository.AddSongToPlaylistAsync(playlistSong);
            return true;
        }

        public async Task<bool> RemoveSongFromPlaylistAsync(int playlistId, int songId)
        {
            await _playlistSongRepository.RemoveSongFromPlaylistAsync(playlistId, songId);
            return true;
        }

        public async Task<List<Song>> GetSongsByPlaylistIdAsync(int playlistId)
        {
            return await _playlistSongRepository.GetSongsByPlaylistIdAsync(playlistId);
        }
    }
}
