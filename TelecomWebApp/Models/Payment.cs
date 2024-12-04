namespace TelecomWebApp.Models
{
    public class Payment
    {
        public int paymentID { get; set; }
        public double amount { get; set; }
            
        public DateTime date_of_payment { get; set; }
        public string payment_method { get; set; }
        public string status { get; set; }
        public string mobileNo { get; set; }

    }
}
