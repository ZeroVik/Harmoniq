using Harmoniq.Enums;

namespace Harmoniq.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        // Foreign key to the user
        public int UserId { get; set; }
        public User User { get; set; }

        // Subscription details
        public SubscriptionPlanEnum Plan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // Null if the subscription is ongoing or auto-renewed
        public bool IsActive { get; set; }

        // Optional: add payment or billing details
        public string PaymentTransactionId { get; set; }
        public DateTime? NextBillingDate { get; set; }
    }

}
