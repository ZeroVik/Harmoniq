using Harmoniq.Enums;

namespace Harmoniq.Dtos.SubscriptionDtos
{
    public class UpdateSubscriptionDto
    {
        public SubscriptionPlanEnum? Plan { get; set; } // ✅ Enum should be nullable for updates
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
