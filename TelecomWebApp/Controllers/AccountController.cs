﻿using System.Diagnostics;
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
            UnresolvedTickets = 5,//_telecomContext .GetUnresolvedTickets(mobileNo),
            HighestValueVoucher = 0//_telecomContext .GetHighestValueVoucher(mobileNo)
        };

        return View("LoggedInCustomerView", serviceInfo);
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

    public IActionResult ViewAllCashbackTransactions()
    {
        int nationalID=0;
        var cashbackTransactions = _telecomContext.GetCashbackTransactions(nationalID);
        return View("CashbackTransactions", cashbackTransactions);
    }
    public IActionResult viewLast5MonthsServicePlans() {
        String mobileNo = "";
        var spmodel =  _telecomContext.GetLast5MonthsServicePlans(mobileNo);
        return View("Last5MonthsServicePlans",spmodel);
    }
}
