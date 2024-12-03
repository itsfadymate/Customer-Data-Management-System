using System.Diagnostics;
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

        return View("Last5MonthsServicePlansView", spmodel);
    }

    public ActionResult RenewSubscriptionView()//for renew button
    {
        
        Debug.WriteLine("AccountController RenewSubscriptionView()");
        return View("RenewSubscriptionView");
    }
    public async Task<IActionResult> RenewSubscription(String mobileNo, decimal amount,int plan_id, String payment_method ) {
        bool success = await _telecomContext.RenewSubscription(mobileNo, amount, payment_method, plan_id);
        return RedirectToAction("Index", "Account");
    }
    public ActionResult CashbackPaymentBenefitView()//for cashback button
    {

        Debug.WriteLine("AccountController CashbackPaymentView()");
        return View("CashbackPaymentBenfitView");
    }
    //temp hardcoded
    public async Task<IActionResult> CashbackPaymentBenefit(int paymentID, int benefitID)
    {

        double val = await _telecomContext.Payment_wallet_cashback("01012345678", paymentID, benefitID);
        return View("CashbackPaymentBenefitView", val);
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
 
}
