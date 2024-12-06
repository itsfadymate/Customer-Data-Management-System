using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using TelecomWebApp.Models;
using System.Diagnostics;

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
        public async Task<IActionResult> AdminLogin(String UserName,String pass)
        {
            ViewData["hidenav"] = true;
            if (UserName == "Admin") {
                if (pass == "Admin")
                {
                    TempData["SuccessMessage"] = "logged in successfully";
                    return View("Dashboard");
                }
                TempData["ErrorMessage"] = "wrong password";
                return View("AdminLogin");
            }
            
            TempData["ErrorMessage"] = "wrong user name";
            return View("AdminLogin");
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
        [HttpGet]
        public IActionResult CashbackFunction()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CashbackFunction(int walletID, int planID)
        {
            int cashbackAmount = -1;
            try{
                cashbackAmount = _context.Database.SqlQuery<int>($"SELECT dbo.Wallet_Cashback_Amount({walletID},{planID}) as Value").FirstOrDefault();
                TempData["SuccessMessage"] = "Data Fetched";
        } catch (Exception e)
            {
                TempData["ErrorMessage"] = "Invalid Data Entered";
            }
            
            ViewBag.CashbackAmount = cashbackAmount;
            return View();
        }
        [HttpGet]
        public IActionResult TransactionAverage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransactionAverage(int walletID, DateTime startDate, DateTime endDate)
        {
            int average = 0;
            try
            { 
                average = _context.Database.SqlQuery<int>($"SELECT dbo.Wallet_Transfer_Amount({walletID},{startDate},{endDate}) as Value").FirstOrDefault();
            } catch(Exception e)
            {
                TempData["ErrorMessage"] = "Invalid Data entered";
            }           
            ViewBag.TransactionAverage = average;
            return View();
        }
        [HttpGet]
        public IActionResult IsWalletLinked()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IsWalletLinked(string mobileNo)
        {
            bool linked = false;
            try
            {
                linked = _context.Database.SqlQuery<bool>($"SELECT dbo.Wallet_MobileNo({mobileNo}) as Value").FirstOrDefault();
                Debug.WriteLine("WALLET LINK CHECK NUMBER" + mobileNo);
            } catch (Exception e)
            {
                TempData["ErrorMessage"] = "Invalid Data entered";
            }
            ViewBag.IsLinked = linked;
            return View();
        }
        [HttpGet]
        public IActionResult AccountPaymentPoints()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AccountPaymentPoints(string mobileNo)
        {
            var result = await _context.PaymentPointsResults.FromSqlInterpolated($"EXEC Account_Payment_Points @mobile_num = {mobileNo}")
                .ToListAsync();
            ViewBag.transactions = result.First().transactions;
            ViewBag.points = result.First().points;
            return View();
        }
        [HttpGet]
        public IActionResult UpdateAccountPoints()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAccountPoints(string mobileNo)
        {
            var mobileNumPar = new SqlParameter("@mobile_num", mobileNo);
            var totalPt = await _context.Database.ExecuteSqlRawAsync(
                "EXEC Total_Points_Account @mobile_num", mobileNumPar);
            ViewBag.TotalPoints = totalPt;
            return View();
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
       
