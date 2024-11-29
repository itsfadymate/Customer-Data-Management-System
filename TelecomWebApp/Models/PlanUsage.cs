namespace TelecomWebApp.Models
{
    public class PlanUsage
    {
        public int UsageID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DataConsumption { get; set; }
        public int MinutesUsed { get; set; }
        public int SMSSent { get; set; }
        public string MobileNo { get; set; }
        public int PlanID { get; set; }
    }
}
