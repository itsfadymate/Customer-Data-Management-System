namespace TelecomWebApp.Models
{
    public class CustomerProfileActiveAccount
    {
        public int nationalID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public DateTime date_of_birth { get; set; }
        public string mobileNo { get; set; }
        public string account_type { get; set; }
        public decimal balance { get; set; }
        public string status { get; set; }
        public DateTime start_date { get; set; }
    }
}
