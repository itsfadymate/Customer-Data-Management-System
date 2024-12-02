using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult CustomerWallet()
        {
            var walletDetails = _context.Set<dynamic>().FromSqlRaw("SELECT * FROM CustomerWallet").ToList();
            return View(walletDetails);
        }
        public IActionResult E_shopVouchers()
        {
            var vouchers = _context.Set<dynamic>().FromSqlRaw("SELECT * FROM E_shopVouchers").ToList();
            return View(vouchers);
        }
        public IActionResult AccountPayments()
        {
            var payments = _context.Set<dynamic>().FromSqlRaw("SELECT * FROM AccountPayments").ToList();
            return View(payments);
        }

        public IActionResult Num_of_cashback()
        {
            var cashbacks = _context.Set<dynamic>().FromSqlRaw("SELECT * FROM Num_of_cashback").ToList();
            return View(cashbacks);
        }
        public async Task<IActionResult> CustomerProfilesWithActiveAccounts()
        {
            var data = await _context.GetCustomerProfilesWithActiveAccountsAsync();
            return View(data);
        }
        public async Task<IActionResult> PhysicalStoreVoucherDetails()
        {
            var data = await _context.GetPhysicalStoreVoucherDetailsAsync();
            return View(data);
        }
        public async Task<IActionResult> ResolvedTickets()
        {
            var data = await _context.GetResolvedTicketsAsync();
            return View(data);
        }
    }
}
       
