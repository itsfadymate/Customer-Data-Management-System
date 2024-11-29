namespace TelecomWebApp.Models
{
    public class Voucher
    {
        public int VoucherID { get; set; }
        public int Value { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Points { get; set; }
        public string MobileNo { get; set; }
        public int ShopID { get; set; }
        public DateTime? RedeemDate { get; set; }
    }
}
