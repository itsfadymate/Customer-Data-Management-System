using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelecomWebApp.Models;

public class AccountController : Controller
{

	private TelecomContext _telecomContext;
	public AccountController(TelecomContext dbContext)
	{
		_telecomContext = dbContext;
    }

	public IActionResult Index()
    {
        var serviceInfo = new ServiceInfo
        {
            RemainingMoney = _context.GetRemainingMoney(),
            ExtraMoney = _context.GetExtraMoney(),
            UnresolvedTickets = _context.GetUnresolvedTickets(),
            HighestValueVoucher = _context.GetHighestValueVoucher()
        };

        return View("LoggedInCustomerView", serviceInfo);
    }
	

}
