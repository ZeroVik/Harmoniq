using Harmoniq.Enums;

namespace Harmoniq.Dtos.SubscriptionDtos
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public SubscriptionPlanEnum Plan { get; set; } // ✅ Changed from string to enum
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // Nullable for ongoing subscriptions
        public bool IsActive { get; set; }
        public DateTime? NextBillingDate { get; set; }
    }
}
