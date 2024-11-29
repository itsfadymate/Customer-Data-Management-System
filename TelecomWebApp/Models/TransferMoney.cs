namespace TelecomWebApp.Models
{
    public class TransferMoney
    {
        public int WalletID1 { get; set; }
        public int WalletID2 { get; set; }
        public int TransferID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
