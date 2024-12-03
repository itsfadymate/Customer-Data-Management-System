namespace TelecomWebApp.Models
{
    public class AccountPayment
    {
        public int paymentID { get; set; }
        public decimal amount { get; set; }
        public DateTime date_of_payment { get; set; }
        public string payment_method { get; set; }
        public string status { get; set; }
        public string mobileNo { get; set; }
    }
}
