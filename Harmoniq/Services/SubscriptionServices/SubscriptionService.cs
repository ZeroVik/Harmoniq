using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.SubscriptionDtos;
using Harmoniq.Enums;
using Harmoniq.Models;
using Harmoniq.Repositories;
using Harmoniq.Repositories.SubscriptionRepositories;
using Harmoniq.Repositories.UserRepositories;

namespace Harmoniq.Services
{
    public class SubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IUserRepository userRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync()
        {
            var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
            return subscriptions.Select(s => new SubscriptionDto
            {
                Id = s.Id,
                UserId = s.UserId,
                Plan = s.Plan, // ✅ Now using enum
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                IsActive = s.IsActive,
                NextBillingDate = s.NextBillingDate
            }).ToList();
        }

        public async Task<SubscriptionDto> GetSubscriptionByIdAsync(int id)
        {
            var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);
            if (subscription == null) return null;

            return new SubscriptionDto
            {
                Id = subscription.Id,
                UserId = subscription.UserId,
                Plan = subscription.Plan,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsActive = subscription.IsActive,
                NextBillingDate = subscription.NextBillingDate
            };
        }

        public async Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User does not exist.");

            var subscription = new Subscription
            {
                UserId = dto.UserId,
                Plan = dto.Plan, // ✅ Now using enum
                StartDate = DateTime.UtcNow,
                IsActive = true,
                NextBillingDate = DateTime.UtcNow.AddMonths(1) // Default to monthly billing
            };

            await _subscriptionRepository.AddSubscriptionAsync(subscription);

            return new SubscriptionDto
            {
                Id = subscription.Id,
                UserId = subscription.UserId,
                Plan = subscription.Plan,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsActive = subscription.IsActive,
                NextBillingDate = subscription.NextBillingDate
            };
        }

        public async Task UpdateSubscriptionAsync(int id, UpdateSubscriptionDto dto)
        {
            var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);
            if (subscription == null) throw new Exception("Subscription not found.");

            subscription.Plan = dto.Plan ?? subscription.Plan;
            subscription.EndDate = dto.EndDate ?? subscription.EndDate;
            subscription.IsActive = dto.IsActive ?? subscription.IsActive;

            await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
        }

        public async Task DeleteSubscriptionAsync(int id)
        {
            var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);
            if (subscription == null) throw new Exception("Subscription not found.");

            await _subscriptionRepository.DeleteSubscriptionAsync(id);
        }
    }
}
