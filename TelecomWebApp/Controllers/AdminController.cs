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

        public IActionResult Num_of_cashback()
        {
            var cashbacks = _context.Num_Of_Cashbacks.FromSqlRaw("SELECT * FROM Num_of_cashback").ToList();
            return View(cashbacks);
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
        //public async Task<IActionResult> ResolvedTickets()
        //{
        //    var data = await _context.GetResolvedTicketsAsync();
        //    return View(data);
        //}
        public async Task<IActionResult> CustomerAccountsWithPlans()
        {
            var data = await _context.GetCustomerAccountsWithPlansAsync();
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










    }
}
       
