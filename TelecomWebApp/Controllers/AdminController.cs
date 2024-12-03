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
        public async Task<IActionResult> AccountPayments()
        {
            var payments = await _context.AccountPayments.FromSqlRaw("SELECT * FROM AccountPayments").ToListAsync();
            return View(payments);
        }

        public async Task<IActionResult> Num_of_cashbacks()
        {
            var cashbacks = await _context.Num_Of_Cashbacks.FromSqlRaw("SELECT * FROM Num_of_cashback").ToListAsync();
            return View(cashbacks);
        }
        public async Task<IActionResult> CustomerProfilesWithActiveAccounts()
        {
            var data = await _context.CustomerProfileActiveAccounts.FromSqlRaw("SELECT * FROM allCustomerAccounts").ToListAsync();
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
       
