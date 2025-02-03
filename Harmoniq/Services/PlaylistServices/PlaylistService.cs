using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harmoniq.Dtos.PlaylistDtos;
using Harmoniq.Dtos;
using Harmoniq.Models;
using Harmoniq.Repositories;
using Harmoniq.Repositories.PlaylistRepositories;
using Harmoniq.Repositories.UserRepositories;

namespace Harmoniq.Services
{
    public class PlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;

        public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository)
        {
            _playlistRepository = playlistRepository ?? throw new ArgumentNullException(nameof(playlistRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IEnumerable<PlaylistDto>> GetAllPlaylistsAsync()
        {
            var playlists = await _playlistRepository.GetAllPlaylistsAsync();
            return playlists.Select(p => new PlaylistDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                UserId = p.UserId
            }).ToList();
        }

        public async Task<PlaylistDto> GetPlaylistByIdAsync(int id)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
            if (playlist == null) return null;

            return new PlaylistDto
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                CreatedAt = playlist.CreatedAt,
                UserId = playlist.UserId
            };
        }

        public async Task<PlaylistDto> CreatePlaylistAsync(CreatePlaylistDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User does not exist.");

            var playlist = new Playlist
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _playlistRepository.AddPlaylistAsync(playlist);

            return new PlaylistDto
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                CreatedAt = playlist.CreatedAt,
                UserId = playlist.UserId
            };
        }

        public async Task UpdatePlaylistAsync(int id, UpdatePlaylistDto dto)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
            if (playlist == null) throw new Exception("Playlist not found.");

            playlist.Name = dto.Name ?? playlist.Name;
            playlist.Description = dto.Description ?? playlist.Description;

            await _playlistRepository.UpdatePlaylistAsync(playlist);
        }

        public async Task DeletePlaylistAsync(int id)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
            if (playlist == null) throw new Exception("Playlist not found.");

            await _playlistRepository.DeletePlaylistAsync(id);
        }
    }
}
