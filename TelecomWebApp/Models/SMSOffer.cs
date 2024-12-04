namespace TelecomWebApp.Models
{
    public class SMSOffer
    {
        public int offerID { get; set; }             
        public string offer_description { get; set; } 
        public DateTime validity_date { get; set; }   
        public string status { get; set; }            
        public string mobileNo { get; set; }         
    }
}
