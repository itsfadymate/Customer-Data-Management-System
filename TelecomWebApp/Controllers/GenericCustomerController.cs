using System;
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
        Debug.WriteLine("Generic customer Index()");
        return View("GenericCustomerView");
    }

    public IActionResult RedirectToLogin() {
        Debug.WriteLine("Generic customer RedirectToLogin()");
        ViewData["hidenav"] = true;
	return View("login");
    }

    [HttpPost]
    public IActionResult Login(String mobileNo,String pass)
    {
        Debug.WriteLine("Generic customer login()");
        if (_telecomContext.login(mobileNo, pass)){
            Debug.WriteLine("valid creadentials");
            TempData["ToastrMessage"] = "Login successful";
	        TempData["ToastrType"] = "success";
            return RedirectToAction("Index","Account");
	}
        Debug.WriteLine("invalid creadentials");
        TempData["ToastrMessage"] = "Credentials not found";
        TempData["ToastrType"] = "error";
        ViewData["hidenav"] = true;
        return View("login");
    }

}
