using Harmoniq.Dtos.UserDtos;
using Harmoniq.Models;
using Harmoniq.Repositories.UserRepositories;

namespace Harmoniq.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl
            });
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            string savedFileName = await FileUploadUtility.SaveFileAsync(createUserDto.ProfileImage, uploadFolder);
            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = HashPassword(createUserDto.Password), // Replace with secure hashing
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                ProfileImageUrl = savedFileName,
                LastUsernameChange = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Id = user.Id, // Assuming EF Core sets this after SaveChangesAsync()
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImageUrl = savedFileName
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public async Task UpdateUserProfileAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (!string.IsNullOrEmpty(updateUserDto.Username) && updateUserDto.Username != user.Username)
            {
                // If the username has been changed before, check the date
                if (user.LastUsernameChange.HasValue &&
                    user.LastUsernameChange.Value.AddYears(1) > DateTime.UtcNow)
                {
                    var nextAvailableChange = user.LastUsernameChange.Value.AddYears(1).ToShortDateString();
                    throw new Exception($"Username can only be changed once per year. Next change available on {nextAvailableChange}.");
                }

                // Otherwise, update the username and set the change date to now
                user.Username = updateUserDto.Username;
                user.LastUsernameChange = DateTime.UtcNow;
            }

            // Update properties if provided
            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.LastName = updateUserDto.LastName ?? user.LastName;

            // ✅ Only update the image if a new file is provided
            if (updateUserDto.ProfileImage != null) // Ensure it's an IFormFile
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Delete the old image before replacing it
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    string oldFilePath = Path.Combine(uploadFolder, user.ProfileImageUrl);
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                // ✅ Pass the IFormFile object correctly
                string fileName = await FileUploadUtility.SaveFileAsync(updateUserDto.ProfileImage, uploadFolder);
                user.ProfileImageUrl = fileName;
            }

            await _userRepository.UpdateAsync(user);
        }



        public async Task<bool> DeleteUserAsync(int Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            if (user == null)
                return false; // User not found

            // Delete the profile image if it exists
            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string filePath = Path.Combine(uploadFolder, user.ProfileImageUrl);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            await _userRepository.DeleteUserAsync(Id);
            return true;
        }

        
    }
}
