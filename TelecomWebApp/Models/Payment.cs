namespace TelecomWebApp.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfPayment { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string MobileNo { get; set; }
    }
}
