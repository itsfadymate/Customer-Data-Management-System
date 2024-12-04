namespace TelecomWebApp.Models
{
    public class CustomerAccountWithPlanDetail
    {
        public string mobileNo { get; set; }          
        public string pass { get; set; }                 
        public decimal balance { get; set; }          
        public string account_type { get; set; }          
        public DateTime start_date { get; set; }   
        
        public string status { get; set; }                              
        public int nationalID { get; set; }               
        public string name { get; set; }            
        public string description { get; set; }     
        public int planID { get; set; }                
        public int SMS_offered { get; set; }             
        public int minutes_offered { get; set; }         
        public int data_offered { get; set; }            
        public int price { get; set; }                   
    }
}
