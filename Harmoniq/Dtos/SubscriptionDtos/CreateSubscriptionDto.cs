using Harmoniq.Enums;

namespace Harmoniq.Dtos.SubscriptionDtos
{
    public class CreateSubscriptionDto
    {
        public int UserId { get; set; }
        public SubscriptionPlanEnum Plan { get; set; }
    }
}
