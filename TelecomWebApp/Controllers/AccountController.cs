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

	public IActionResult GetUnresolvedTickets(int nationalID)
	{
		var unresolved = _telecomContext.GetUnresolvedTickets(nationalID);

        return View(unresolved);
	}
	

}
