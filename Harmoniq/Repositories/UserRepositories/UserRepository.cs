using Harmoniq.Enums;
using Harmoniq.Models;
using Harmoniq.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Harmoniq.Repositories.UserRepositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                _dbSet.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllAdminsAsync()
        {
            return await _context.Users.Where(u => u.Role == UserRoleEnum.Admin).ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
