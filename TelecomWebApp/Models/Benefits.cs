namespace TelecomWebApp.Models
{
    public class Benefits
    {
        public int BenefitID { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ValidityDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
    }
}
