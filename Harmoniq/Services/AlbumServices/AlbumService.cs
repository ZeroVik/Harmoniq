using Harmoniq.Dtos.AlbumDtos;
using Harmoniq.Models;
using Harmoniq.Repositories;
using Harmoniq.Repositories.ArtistRepositories;

namespace Harmoniq.Services.AlbumServices
{
    public class AlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly string _uploadFolderPath;

        public AlbumService(IAlbumRepository albumRepository, IArtistRepository artistRepository, IConfiguration configuration)
        {
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "albums");
        }

        public async Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync()
        {
            var albums = await _albumRepository.GetAllAlbumsAsync();
            return albums.Select(a => new AlbumDto
            {
                Id = a.Id,
                Title = a.Title,
                ReleaseDate = a.ReleaseDate,
                AlbumArtUrl = a.AlbumArtUrl,
                ArtistId = a.ArtistId
            }).ToList();
        }

        public async Task<AlbumDto> GetAlbumByIdAsync(int id)
        {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null) return null;

            return new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                AlbumArtUrl = album.AlbumArtUrl,
                ArtistId = album.ArtistId
            };
        }

        public async Task<AlbumDto> CreateAlbumAsync(CreateAlbumDto dto)
        {
            if (!await _artistRepository.ExistsAsync(dto.ArtistId))
                throw new Exception("Artist does not exist.");

            string fileName = await SaveFileAsync(dto.AlbumArt);

            var album = new Album
            {
                Title = dto.Title,
                ReleaseDate = dto.ReleaseDate,
                ArtistId = dto.ArtistId,
                AlbumArtUrl = fileName
            };

            await _albumRepository.AddAlbumAsync(album);

            return new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate,
                AlbumArtUrl = fileName,
                ArtistId = album.ArtistId
            };
        }

        public async Task UpdateAlbumAsync(int id, UpdateAlbumDto dto)
        {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null) throw new Exception("Album not found.");

            album.Title = dto.Title ?? album.Title;
            album.ReleaseDate = dto.ReleaseDate ?? album.ReleaseDate;

            if (dto.AlbumArt != null)
            {
                if (!string.IsNullOrEmpty(album.AlbumArtUrl))
                {
                    string oldFilePath = Path.Combine(_uploadFolderPath, album.AlbumArtUrl);
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                string fileName = await SaveFileAsync(dto.AlbumArt);
                album.AlbumArtUrl = fileName;
            }

            await _albumRepository.UpdateAlbumAsync(album);
        }

        public async Task DeleteAlbumAsync(int id)
        {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null) throw new Exception("Album not found.");

            string filePath = Path.Combine(_uploadFolderPath, album.AlbumArtUrl);
            if (File.Exists(filePath))
                File.Delete(filePath);

            await _albumRepository.DeleteAlbumAsync(id);
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
