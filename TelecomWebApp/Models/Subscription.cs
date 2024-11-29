namespace TelecomWebApp.Models
{
    public class Subscription
    {
        public string MobileNo { get; set; } 
        public int PlanID { get; set; } 
        public DateTime SubscriptionDate { get; set; } 
        public string Status { get; set; } 
    }
}
