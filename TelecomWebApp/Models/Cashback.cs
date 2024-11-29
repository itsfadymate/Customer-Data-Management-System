namespace TelecomWebApp.Models
{
    public class Cashback
    {
        public int CashbackID { get; set; }
        public int BenefitID { get; set; }
        public int WalletID { get; set; }
        public int Amount { get; set; }
        public DateTime CreditDate { get; set; }
    }
}
