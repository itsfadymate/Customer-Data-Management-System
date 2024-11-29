namespace TelecomWebApp.Models
{
    public class ServicePlan
    {
        public int PlanID { get; set; }
        public int SMSOffered { get; set; }
        public int MinutesOffered { get; set; }
        public int DataOffered { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
    }
}
