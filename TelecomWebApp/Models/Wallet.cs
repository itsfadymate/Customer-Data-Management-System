using System.Security.Principal;

namespace TelecomWebApp.Models
{
    public class Wallet
    {
        public int walletID { get; set; }
        public decimal current_balance { get; set; }
        public string currency { get; set; }
        public DateTime last_modified_date { get; set; }
        public int nationalID { get; set; }
        public string mobileNo { get; set; }
    }
}
