namespace TelecomWebApp.Models
{
    public class CustomerAccount
    {
        public int NationalID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
