namespace TelecomWebApp.Models
{
    public class CustomerAccountWithPlan
    {
        public string mobileNo { get; set; }          
        public string pass { get; set; }                 
        public decimal balance { get; set; }          
        public string account_type { get; set; }          
        public DateTime start_date { get; set; }         
        public string status { get; set; }             
        public int point { get; set; }                   
        public int nationalID { get; set; }               
        public string planName { get; set; }            
        public DateTime subscription_date { get; set; }  
        public string plan_description { get; set; }     
        public int planID { get; set; }                
        public int SMS_offered { get; set; }             
        public int minutes_offered { get; set; }         
        public int data_offered { get; set; }            
        public int price { get; set; }                   
    }
}
