using Harmoniq.Models;

namespace Harmoniq.Repositories.SubscriptionRepositories
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
        Task<Subscription> GetSubscriptionByIdAsync(int id);
        Task<Subscription> GetSubscriptionByUserIdAsync(int userId);
        Task AddSubscriptionAsync(Subscription subscription);
        Task UpdateSubscriptionAsync(Subscription subscription);
        Task DeleteSubscriptionAsync(int id);
    }
}
