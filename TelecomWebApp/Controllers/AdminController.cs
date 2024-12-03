using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelecomWebApp.Models;

namespace TelecomWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly TelecomContext _context;
        public AdminController(TelecomContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> CustomerWallet()
        {
            var walletDetails = await _context.CustomerWallets.FromSqlRaw("SELECT * FROM CustomerWallet").ToListAsync();
            return View(walletDetails);
        }
        public async Task<IActionResult> E_shopVouchers()
        {
            var vouchers = await _context.E_ShopVouchers.FromSqlRaw("SELECT * FROM E_shopVouchers").ToListAsync();
            return View(vouchers);
        }
        public IActionResult AccountPayments()
        {
            var payments = _context.AccountPayments.FromSqlRaw("SELECT * FROM AccountPayments").ToList();
            return View(payments);
        }

        public async Task<IActionResult> Num_of_cashbacks()
        {
            var data = await _context.Num_Of_Cashbacks.FromSqlRaw("SELECT * FROM Num_of_cashback").ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> CustomerProfilesWithActiveAccounts()
        {
            var data = await _context.CustomerProfileActiveAccounts.FromSqlRaw("SELECT * FROM allCustomerAccounts").ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> PhysicalStoreVoucherDetails()
        {
            var data = await _context.PhysicalStoreVoucherDetails.FromSqlRaw("SELECT * FROM PhysicalStoreVouchers").ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> ResolvedTickets()
        {
            var data = await _context.ResolvedTicketDetails.FromSqlRaw("SELECT * FROM allResolvedTickets").ToListAsync();
            return View(data);
        }


        public async Task<IActionResult> CustomerAccountWithPlanDetails()
        {
            var data = await _context.CustomerAccountWithPlanDetails.FromSqlRaw("EXEC Account_Plan").ToListAsync();
            return View(data);
        }
        [HttpGet]
        public IActionResult CustomerAccountsByPlanDate()
        {
            return View(); // Returns a form for input
        }

        [HttpPost]
        public async Task<IActionResult> CustomerAccountsByPlanDate(DateTime subscriptionDate, int planId)
        {
            var data = await _context.GetCustomerAccountsByPlanDateAsync(subscriptionDate, planId);
            return View("CustomerAccountsByPlanDateResult", data); // Displays results
        }
        [HttpGet]
        public IActionResult AccountUsagePlan()
        {
            return View(); // Returns a form for input
        }

        [HttpPost]
        public async Task<IActionResult> AccountUsagePlan(string mobileNum, DateTime startDate)
        {
            var data = await _context.GetAccountUsagePlanAsync(mobileNum, startDate);
            return View("AccountUsagePlanResult", data); // Displays the results
        }
        [HttpGet]
        public IActionResult RemoveBenefits()
        {
            return View(); // Returns the input form view
        }

        // POST: Executes the stored procedure to remove benefits for the given input
        [HttpPost]
        public async Task<IActionResult> RemoveBenefits(RemoveBenefit input)
        {
            if (!ModelState.IsValid)
            {
                return View(input); 
            }

            
            await _context.RemoveBenefitsAsync(input.mobileNo, input.planID);

            
            TempData["Message"] = "Benefits removed successfully.";
            return RedirectToAction("RemoveBenefits"); 
        }
        [HttpGet]
        public IActionResult GetSMSOffers()
        {
            return View(); // Returns the input form view
        }

        // POST: Fetches the SMS offers for the given MobileNo and displays them
        [HttpPost]
        public async Task<IActionResult> GetSMSOffers(string mobileNo)
        {
            if (!ModelState.IsValid)
            {
                return View(mobileNo); // Return the form with validation errors
            }

            // Fetch the SMS offers using the function
            var smsOffers = await _context.GetSMSOffersAsync(mobileNo);

            // Pass the result to the view
            return View("SMSOffersResult", smsOffers); // Displays the result view
        }








    }
}
       
