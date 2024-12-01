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

	public IActionResult Index(String mobileNo)
    {
        var serviceInfo = new ServiceInfo
        {
            UnresolvedTickets = _telecomContext .GetUnresolvedTickets(mobileNo),
            HighestValueVoucher = _telecomContext .GetHighestValueVoucher(mobileNo)
        };

        return View("LoggedInCustomerView", serviceInfo);
    }
	

}
