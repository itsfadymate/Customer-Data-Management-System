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
            HttpContext.Session.SetString("MobileNo", mobileNo);
            Debug.WriteLine("valid creadentials");
            TempData["SuccessMessage"] = "Login successful";
	        
            return RedirectToAction("Index","Account");
	}
        Debug.WriteLine("invalid creadentials");
        TempData["ErrorMessage"] = "Credentials not found";
        ViewData["hidenav"] = true;
        return View("login");
    }

    public async Task<IActionResult> AllBenefitsView()
    {
        Debug.WriteLine("Generic AllBenefitsView()");
        
        try
        {
            var benefits = await _telecomContext.GetAllActiveBenefits();
            return View("AllActiveBenefitsView", benefits);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "couldn't retrieve active benfits";
            Debug.WriteLine(e.Message);
        }
        return View("AllActiveBenefitsView");
    }

}
