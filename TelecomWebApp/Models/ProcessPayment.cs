namespace TelecomWebApp.Models
{
    public class ProcessPayment
    {
        public int PaymentID { get; set; }
        public int PlanID { get; set; }
        public decimal RemainingBalance { get; set; }
        public decimal ExtraAmount { get; set; }
    }
}
