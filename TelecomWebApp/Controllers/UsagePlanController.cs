using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TelecomWebApp.Models;

namespace TelecomWebApp.Controllers
{
    public class UsagePlanController : Controller
    {
        private readonly TelecomContext _dbContext;

        public UsagePlanController(TelecomContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(string mobileNo)
        {
            var usagePlans = await _dbContext.GetUsagePlanCurrentMonthAsync(mobileNo);
            return View(usagePlans);
        }
    }

}
