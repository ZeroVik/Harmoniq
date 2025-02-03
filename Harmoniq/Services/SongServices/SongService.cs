using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Harmoniq.Dtos.SongDtos;
using Harmoniq.Dtos;
using Harmoniq.Models;
using Harmoniq.Repositories;
using Harmoniq.Repositories.SongRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Harmoniq.Services
{
    public class SongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly string _uploadFolderPath;

        public SongService(ISongRepository songRepository, IAlbumRepository albumRepository, IConfiguration configuration)
        {
            _songRepository = songRepository;
            _albumRepository = albumRepository;
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "songs");
        }

        public async Task<IEnumerable<SongDto>> GetAllSongsAsync()
        {
            var songs = await _songRepository.GetAllSongsAsync();
            return songs.Select(s => new SongDto
            {
                Id = s.Id,
                Title = s.Title,
                AlbumId = s.AlbumId,
                Duration = s.Duration,
                TrackNumber = s.TrackNumber,
                FilePath = s.FilePath
            });
        }

        public async Task<SongDto> GetSongByIdAsync(int id)
        {
            var song = await _songRepository.GetSongByIdAsync(id);
            if (song == null) return null;

            return new SongDto
            {
                Id = song.Id,
                Title = song.Title,
                AlbumId = song.AlbumId,
                Duration = song.Duration,
                TrackNumber = song.TrackNumber,
                FilePath = song.FilePath
            };
        }

        public async Task<SongDto> CreateSongAsync(CreateSongDto dto)
        {
            if (!await _albumRepository.ExistsAsync(dto.AlbumId))
                throw new Exception("Album does not exist.");

            string fileName = await SaveFileAsync(dto.SongFile);

            var song = new Song
            {
                Title = dto.Title,
                AlbumId = dto.AlbumId,
                Duration = dto.Duration,
                TrackNumber = dto.TrackNumber,
                FilePath = fileName
            };

            await _songRepository.AddSongAsync(song);

            return new SongDto
            {
                Id = song.Id,
                Title = song.Title,
                AlbumId = song.AlbumId,
                Duration = song.Duration,
                TrackNumber = song.TrackNumber,
                FilePath = fileName
            };
        }

        public async Task UpdateSongAsync(int id, UpdateSongDto dto)
        {
            var song = await _songRepository.GetSongByIdAsync(id);
            if (song == null) throw new Exception("Song not found.");

            song.Title = dto.Title ?? song.Title;
            song.AlbumId = dto.AlbumId ?? song.AlbumId;
            song.Duration = dto.Duration ?? song.Duration;
            song.TrackNumber = dto.TrackNumber ?? song.TrackNumber;

            await _songRepository.UpdateSongAsync(song);
        }

        public async Task DeleteSongAsync(int id)
        {
            var song = await _songRepository.GetSongByIdAsync(id);
            if (song == null) throw new Exception("Song not found.");

            string filePath = Path.Combine(_uploadFolderPath, song.FilePath);
            if (File.Exists(filePath))
                File.Delete(filePath);

            await _songRepository.DeleteSongAsync(id);
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Invalid file.");

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(_uploadFolderPath, fileName);

            Directory.CreateDirectory(_uploadFolderPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
