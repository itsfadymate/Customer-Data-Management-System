namespace TelecomWebApp.Models
{
    public class Wallet
    {
        public int WalletID { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Currency { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int NationalID { get; set; }
        public string MobileNo { get; set; }
    }
}
