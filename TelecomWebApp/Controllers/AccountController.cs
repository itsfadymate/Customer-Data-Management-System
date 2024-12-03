﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TelecomWebApp.Models;

public class AccountController : Controller
{

	private TelecomContext _telecomContext;
	public AccountController(TelecomContext dbContext)
	{
		_telecomContext = dbContext;
    }

	public async Task<IActionResult> Index(String mobileNo)
    {
        var serviceInfo = new ServiceInfo
        {
            UnresolvedTickets = 5,//_telecomContext .GetUnresolvedTickets(mobileNo),
            HighestValueVoucher = 0//await _telecomContext .GetHighestValueVoucher(mobileNo)
        };

        return View("LoggedInCustomerView", serviceInfo);
    }
    public async Task<IActionResult> ViewLast5MonthsServicePlans()
    {
        Debug.WriteLine("AccountController ViewLast5MonthsServicePlans()");
        String mobileNo = "01012345678";
        var spmodel = await _telecomContext.GetLast5MonthsServicePlans(mobileNo);

        return View("Last5MonthsServicePlans", spmodel);
    }


    public IActionResult ViewAllPlans()
    {
        var plans = _telecomContext.GetServicePlans();
        return View("ServicePlan", plans);
    }

    public IActionResult UsageInDuration(string planName, DateTime startDate, DateTime endDate)
    {
        var usage = _telecomContext.GetConsumption(planName, startDate, endDate);
        return View("Consumption", usage);
    }


    public IActionResult ViewAllPlansNotSubbed()
    {
        String mobileNo = "";
        var notSubbed = _telecomContext.GetServicePlansNotSubbed(mobileNo);
        return View("NotSubbed", notSubbed);
    }
   

    /*public async Task<IActionResult> ViewLast5MonthsServicePlans()
    {
        String mobileNo = "01012345678";  // or pass this dynamically

        // Use FromSqlRaw to directly call the stored function and fetch the service plans
        var spmodel = await _telecomContext
            .FromSqlRaw("SELECT * FROM dbo.Subscribed_plans_5_Months(@mobileNo)", new SqlParameter("@mobileNo", mobileNo))
            .ToListAsync();

        Debug.WriteLine("AccountController ViewLast5MonthsServicePlans()");

        // Return the data to the view
        return View("Last5MonthsServicePlans", spmodel);
    }*/






}
