using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.UserDtos;
using Harmoniq.Enums;
using Harmoniq.Models;
using Harmoniq.Repositories.UserRepositories;

namespace Harmoniq.Services
{
    public class AdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllAdminsAsync()
        {
            var admins = await _userRepository.GetAllAdminsAsync();
            return admins.Select(admin => new UserDto
            {
                Id = admin.Id,
                Username = admin.Username,
                Email = admin.Email,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                ProfileImageUrl = admin.ProfileImageUrl
            });
        }

        public async Task PromoteToAdminAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            user.Role = UserRoleEnum.Admin;
            await _userRepository.UpdateAsync(user);
        }

        public async Task DemoteToUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            user.Role = UserRoleEnum.User;
            await _userRepository.UpdateAsync(user);
        }
    }
}
