using Harmoniq.Dtos.ArtistDtos;
using Harmoniq.Models;
using Harmoniq.Repositories.ArtistRepositories;

namespace Harmoniq.Services.ArtistService
{
    public class ArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly string _uploadFolderPath;

        public ArtistService(IArtistRepository artistRepository, IConfiguration configuration)
        {
            _artistRepository = artistRepository;
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "artists");
        }

        public async Task<IEnumerable<ArtistDto>> GetAllArtistsAsync()
        {
            var artists = await _artistRepository.GetAllArtistsAsync();
            return artists.Select(a => new ArtistDto
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio,
                ProfileImageUrl = a.ProfileImageUrl
            }).ToList();
        }

        public async Task<ArtistDto> GetArtistByIdAsync(int id)
        {
            var artist = await _artistRepository.GetArtistByIdAsync(id);
            if (artist == null) return null;

            return new ArtistDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Bio = artist.Bio,
                ProfileImageUrl = artist.ProfileImageUrl
            };
        }

        public async Task<ArtistDto> CreateArtistAsync(CreateArtistDto dto)
        {
            string fileName = await SaveFileAsync(dto.ProfileImage);

            var artist = new Artist
            {
                Name = dto.Name,
                Bio = dto.Bio,
                ProfileImageUrl = fileName
            };

            await _artistRepository.AddArtistAsync(artist);

            return new ArtistDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Bio = artist.Bio,
                ProfileImageUrl = fileName
            };
        }

        public async Task UpdateArtistAsync(int id, UpdateArtistDto dto)
        {
            var artist = await _artistRepository.GetArtistByIdAsync(id);
            if (artist == null) throw new Exception("Artist not found.");

            artist.Name = dto.Name ?? artist.Name;
            artist.Bio = dto.Bio ?? artist.Bio;

            // ✅ Only update the image if a new file is provided
            if (dto.ProfileImage != null)
            {
                // Delete the old image before replacing it
                if (!string.IsNullOrEmpty(artist.ProfileImageUrl))
                {
                    string oldFilePath = Path.Combine(_uploadFolderPath, artist.ProfileImageUrl);
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                string fileName = await SaveFileAsync(dto.ProfileImage);
                artist.ProfileImageUrl = fileName;
            }

            await _artistRepository.UpdateArtistAsync(artist);
        }


        public async Task DeleteArtistAsync(int id)
        {
            var artist = await _artistRepository.GetArtistByIdAsync(id);
            if (artist == null) throw new Exception("Artist not found.");

            string filePath = Path.Combine(_uploadFolderPath, artist.ProfileImageUrl);
            if (File.Exists(filePath))
                File.Delete(filePath);

            await _artistRepository.DeleteArtistAsync(id);
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
