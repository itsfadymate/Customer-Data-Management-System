namespace TelecomWebApp.Models
{
    public class PhysicalStoreVoucherDetails
    {
        public int shopID { get; set; }             
        public string name { get; set; }             
        public string category { get; set; }      
        public string address { get; set; }        
        public string working_hours { get; set; }     
        public int voucherID { get; set; }           
        public int value { get; set; }                  
        public DateTime expiry_date { get; set; }       
        public DateTime redeem_date { get; set; }      
        public int points { get; set; }                 
        public string mobileNo { get; set; }            
    }
}
