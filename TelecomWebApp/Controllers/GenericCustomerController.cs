using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TelecomWebApp.Models;

public class GenericCustomerController : Controller
{
    private TelecomContext _telecomContext;
    public GenericCustomerController(TelecomContext dbContext)
    {
        _telecomContext = dbContext;
    }
    public IActionResult Index() {
		return View("GenericCustomerView");
    }

    public IActionResult redirectToLogin() {
	ViewData["hidenav"] = true;
	return View("login");
    }
    public IActionResult login(String mobileNo,String pass)
    {
        if (_telecomContext.login(mobileNo, pass)){
	    TempData["ToastrMessage"] = "Login successful";
	    TempData["ToastrType"] = "success";
            return RedirectToAction("Index","Account");
	}
	    TempData["ToastrMessage"] = "Credentials not found";
	    TempData["ToastrType"] = "error"
        return View("login");
    }

}
