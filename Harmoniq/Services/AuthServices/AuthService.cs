using System;
using System.Threading.Tasks;
using Harmoniq.Dtos.AuthDtos;
using Harmoniq.Dtos;
using Harmoniq.Models;
using Harmoniq.Repositories.UserRepositories;
using Harmoniq.Utilities;
using Microsoft.Extensions.Configuration;

namespace Harmoniq.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;
        private readonly int _jwtExpirationMinutes;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecret = configuration["JwtSettings:SecretKey"];
            _jwtExpirationMinutes = int.Parse(configuration["JwtSettings:ExpirationMinutes"]);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("User already exists with this email.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ProfileImageUrl = "default.jpg",
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            var token = JwtUtility.GenerateJwtToken(user.Id, user.Username, _jwtSecret, _jwtExpirationMinutes);

            return new AuthResponseDto { Token = token, Username = user.Username };
        }


        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password.");

            var token = JwtUtility.GenerateJwtToken(user.Id, user.Username, _jwtSecret, _jwtExpirationMinutes);

            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username
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
    }
}
