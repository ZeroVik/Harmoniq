using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harmoniq.Models;
using Harmoniq.Repositories.SubscriptionRepositories;
using Microsoft.EntityFrameworkCore;

namespace Harmoniq.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _context.Subscriptions.Include(s => s.User).ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(int id)
        {
            return await _context.Subscriptions.Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Subscription> GetSubscriptionByUserIdAsync(int userId)
        {
            return await _context.Subscriptions.Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive);
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubscriptionAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubscriptionAsync(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }
    }
}
