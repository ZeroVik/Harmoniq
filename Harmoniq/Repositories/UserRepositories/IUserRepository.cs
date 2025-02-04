using System.Threading.Tasks;
using Harmoniq.Models;
using Harmoniq.Repositories.Generic;

namespace Harmoniq.Repositories.UserRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task DeleteUserAsync(int userId);
        Task<IEnumerable<User>> GetAllAdminsAsync();
    }
}
