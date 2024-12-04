using System.Diagnostics;
using System.Reflection.Metadata;
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

	public async Task<IActionResult> Index()
    {
        Debug.WriteLine("Account Index()");
        int unresolvedCount = 0;
        int HighestValueVoucherID = 0;
        String MobileNo = HttpContext.Session.GetString("MobileNo");
        try
        {
             unresolvedCount = _telecomContext.GetUnresolvedTickets(MobileNo);
             HighestValueVoucherID = (await _telecomContext.GetHighestValueVoucher(MobileNo)).voucherID;
        }
        catch (Exception ex) {
            //Debug.WriteLine(ex.Message);
            Debug.WriteLine("------------------------- couldn't read from the database unresolvedCount & highestVoucherID");
        }

        var serviceInfo = new ServiceInfo
        {
            UnresolvedTickets = unresolvedCount,
            HighestValueVoucher = HighestValueVoucherID
        };

        return View("LoggedInCustomerView", serviceInfo);
    }
    public async Task<IActionResult> ViewLast5MonthsServicePlans()
    {
        Debug.WriteLine("AccountController ViewLast5MonthsServicePlans()");
        String mobileNo = HttpContext.Session.GetString("MobileNo"); 
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
    
    public async Task<IActionResult> CashbackPaymentBenefit(int paymentID, int benefitID)
    {
        String MobileNo = HttpContext.Session.GetString("MobileNo");

        double val = await _telecomContext.Payment_wallet_cashback(MobileNo, paymentID, benefitID);
        return View("CashbackPaymentBenefitView", val);
    }

    public async Task<IActionResult> TopTenPaymentsView()
    {
        Debug.WriteLine("Account TopTenPaymentsView()");
        String mobileNo = HttpContext.Session.GetString("MobileNo");
        ViewData["hidenav"] = true;
        ViewData["hidecontainer"] = true;
        try
        {
            var payments = await _telecomContext.GetTopTenPayments(mobileNo);
            return View("TopTenPaymentsView", payments);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "couldn't retrieve payments";
            Debug.WriteLine(e.Message);
        }
        return View("TopTenPaymentsView");
    }

    public IActionResult ViewAllPlans()
    {
        var plans = _telecomContext.GetServicePlans();
        return View("ServicePlan", plans);
    }
    public IActionResult CheckDueAmountsView() {
        ViewData["hidenav"] = true;
        return View("CheckDueAmountsView");
    }
    public IActionResult CheckDueAmounts(String PlanName)
    {
        String mobileNo = HttpContext.Session.GetString("MobileNo");
        ViewData["hidenav"] = true;
        Debug.WriteLine("AccountController CheckDueAmounts() ");
        
        int dbRemaining = 0;
        int dbExtra = 0;

        try
        {
            dbExtra = _telecomContext.GetExtraPlanAmount(mobileNo, PlanName);
            dbRemaining = _telecomContext.GetRemainingPlanAmount(mobileNo, PlanName);
            Debug.WriteLine($"ExtraAMounts:{dbExtra} RemainingAmounts:{dbRemaining} ");
        }catch (Exception e)
        {
            TempData["ErrorMessage"] = "Invalid plan name";
            Debug.WriteLine(e.Message);
        }
        var DueAmounts = new DueAmounts
        {
            ExtraAmount = dbExtra,
            RemainingAmount = dbRemaining
        };
        return View("CheckDueAmountsView", DueAmounts);
    }

    public IActionResult UsageInDuration(string planName, DateTime startDate, DateTime endDate)
    {
        var usage = _telecomContext.GetConsumption(planName, startDate, endDate);
        return View("Consumption", usage);
    }

    /*public IActionResult UsageCurrMonth() needs fix idk who did it ~fady
    {
        String mobileNo = HttpContext.Session.GetString("MobileNo");
        var usage = _telecomContext.GetUsageCurrMonth(mobileNo);
        return View("UsageCurrMonth", usage);
    }*/

    

    public IActionResult ViewAllPlansNotSubbed()
    {
        String MobileNo = HttpContext.Session.GetString("MobileNo");
        var notSubbed = _telecomContext.GetServicePlansNotSubbed(MobileNo);
        return View("NotSubbed", notSubbed);
    }
    
    public IActionResult ViewAllCashbackTransactions()
    {
        int nationalID=0;
        var cashbackTransactions = _telecomContext.GetCashbackTransactions(nationalID);
        return View("CashbackTransactions", cashbackTransactions);
    }
 
}
