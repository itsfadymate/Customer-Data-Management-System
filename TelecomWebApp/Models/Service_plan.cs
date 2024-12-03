namespace TelecomWebApp.Models
{
    public class Service_plan
    {
        public int planId { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int SMS_offered { get; set; }
        public int minutes_offered { get; set; }
        public int data_offered { get; set; }
        
        public string description { get; set; }

    }


}
