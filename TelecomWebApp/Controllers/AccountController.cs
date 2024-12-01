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


    public IActionResult ViewAllPlans()
    {
        var plans = _telecomContext.GetServicePlans;
        return View("ServicePlan", plans);
    }

    public IActionResult UsageInDuration()
    {
        var usage = _telecomContext.GetConsumption;
        return View("Consumption", usage);
    }


    public IActionResult ViewAllPlansNotSubbed()
    {
        var notSubbed = _telecomContext.GetServicePlansNotSubbed;
        return View("NotSubbed", notSubbed);
    }
}
