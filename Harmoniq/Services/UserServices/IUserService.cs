using Harmoniq.Dtos.UserDtos;

namespace Harmoniq.Services.UserServices
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task <UserDto>CreateUserAsync(CreateUserDto createUserDto);
        Task UpdateUserProfileAsync(int id, UpdateUserDto updateUserDto);
        Task <bool>DeleteUserAsync(int id);
    }
}
