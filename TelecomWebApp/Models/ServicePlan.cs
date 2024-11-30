namespace TelecomWebApp.Models
{
    public class ServicePlan
    {
        public int planId { get; set; }
        public int SMS_offered { get; set; }
        public int minutesOffered { get; set; }
        public int dataOffered { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string description { get; set; }

    }


}
