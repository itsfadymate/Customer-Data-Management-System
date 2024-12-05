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
        ViewData["hidenav"] = true;
        try
        {
            unresolvedCount = _telecomContext.GetUnresolvedTickets(MobileNo);
            HighestValueVoucherID = (await _telecomContext.GetHighestValueVoucher(MobileNo))?.voucherID ?? default;
        }
        catch (Exception ex)
        {
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
        ViewData["hidenav"] = true;
        Debug.WriteLine("AccountController ViewLast5MonthsServicePlans()");
        String mobileNo = HttpContext.Session.GetString("MobileNo");
        try
        {
            var spmodel = await _telecomContext.GetLast5MonthsServicePlans(mobileNo);

            return View("Last5MonthsServicePlansView", spmodel);
        }
        catch (Exception e)
        {
            Debug.WriteLine("   couldn't retrieve plans");
            Debug.WriteLine(e.Message);
            TempData["ErrorMessage"] = "some problem happened with our server, try again later";
            return View("Last5MonthsServicePlansView");
        }
    }

    public ActionResult RenewSubscriptionView()//for renew button
    {
        ViewData["hidenav"] = true;
        Debug.WriteLine("AccountController RenewSubscriptionView()");
        return View("RenewSubscriptionView");
    }
    public async Task<IActionResult> RenewSubscription(String mobileNo, decimal amount, int plan_id, String payment_method)
    {
        Debug.WriteLine("Account RenewSubscription()");
        ViewData["hidenav"] = true;
        bool success = true;
        try
        {
            success = await _telecomContext.RenewSubscription(mobileNo, amount, payment_method, plan_id);

        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            TempData["ErrorMessage"] = "couldn't renew subscription try again later";
            return RedirectToAction("RenewSubscriptionView", "Account");
        }
        if (!success)
        {
            TempData["ErrorMessage"] = "Invalid data entered";
            return RedirectToAction("RenewSubscriptionView", "Account");
        }
        TempData["SuccessMessage"] = "Payment made successfully";
        return RedirectToAction("Index", "Account");
    }
    public ActionResult CashbackPaymentBenefitView()//for cashback button
    {

        ViewData["hidenav"] = true;
        Debug.WriteLine("AccountController CashbackPaymentBenefitView()");
        return View("CashbackPaymentBenefitView");
    }

    public async Task<IActionResult> CashbackPaymentBenefit(int paymentID, int benefitID)
    {
        Debug.WriteLine("AccountController CashbackPaymentBenefit()");
        ViewData["hidenav"] = true;
        String MobileNo = HttpContext.Session.GetString("MobileNo");

        try
        {
            decimal val = await _telecomContext.Payment_wallet_cashback(MobileNo, paymentID, benefitID);
            if (val < 0)
            {
                TempData["ErrorMessage"] = "invalid data entered";
                return View("CashbackPaymentBenefitView");
            }
            TempData["SuccessMessage"] = "loaded successfully";
            return View("CashbackPaymentBenefitView", val);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            TempData["ErrorMessage"] = "Something is wrong with our system try again later";
        }
        return View("CashbackPaymentBenefitView");
    }

    public async Task<IActionResult> TopTenPaymentsView()
    {
        ViewData["hidenav"] = true;
        Debug.WriteLine("Account TopTenPaymentsView()");
        String mobileNo = HttpContext.Session.GetString("MobileNo");
        try
        {
            var payments = await _telecomContext.GetTopTenPayments(mobileNo);
            Debug.WriteLine("   got payments");
            return View("TopTenPaymentsView", payments);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "couldn't retrieve payments";
            Debug.WriteLine(e.Message);
        }
        return View("TopTenPaymentsView");
    }



    public IActionResult CheckDueAmountsView()
    {
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
        }
        catch (Exception e)
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

    [HttpPost]
    public async Task<IActionResult> ConsumptionForm(string plan_name, DateTime start_date, DateTime end_date)
    {
        ViewData["hidenav"] = true;
        var usage = await _telecomContext.GetConsumption(plan_name, start_date, end_date);
        return View("Consumption", usage);
    }

    public IActionResult ConsumptionForm()
    {
        ViewData["hidenav"] = true;
        return View();
    }




    public async Task<IActionResult> UsageCurrMonth()
    {
        ViewData["hidenav"] = true;
        String mobileNo = HttpContext.Session.GetString("MobileNo");
        var usage = await _telecomContext.GetUsagePlanCurrentMonthAsync(mobileNo);
        return View("UsageCurrMonth", usage);
    }



    public async Task<IActionResult> NotSubbed()
    {
        ViewData["hidenav"] = true;
        String MobileNo = HttpContext.Session.GetString("MobileNo");
        var notSubbed = await _telecomContext.GetServicePlansNotSubbed(MobileNo);
        return View("NotSubbed", notSubbed);
    }

    [HttpPost]
    public IActionResult CashbackTransactionsForm(int nationalId)
    {

        return View("CashbackTransactions", nationalId);
    }

    public async Task<IActionResult> CashbackTransactions()
    {
        ViewData["hidenav"] = true;
        String MobileNo = HttpContext.Session.GetString("MobileNo");
        var cashbackTransactions = await _telecomContext.GetCashbackTransactions(MobileNo);
        return View("CashbackTransactions", cashbackTransactions);
    }
    public IActionResult RedeemVoucherView()
    {
        Debug.WriteLine("Account RedeemVoucherView()");
        ViewData["hidenav"] = true;
        return View("RedeemVoucherView");
    }
    public async Task<IActionResult> RedeemVoucher(int VoucherID)
    {
        Debug.WriteLine("Account RedeemVoucher()");
        ViewData["hidenav"] = true;
        String mobileNo = HttpContext.Session.GetString("MobileNo");
        try
        {
            await _telecomContext.RedeemVoucher(mobileNo, VoucherID);
            TempData["SuccessMessage"] = "Redeemed successfully";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "couldn't redeem Voucher";
        }
        return View("RedeemVoucherView");
    }

    public IActionResult RechargeBalanceView() {
        ViewData["hidenav"] = true;
        Debug.WriteLine("Account RechargeBalanceView()");
        return View("RechargeBalanceView");
    }
    public async Task<IActionResult> RechargeBalance(decimal paymentAmount, String paymentMethod)
    {

        ViewData["hidenav"] = true;
        String MobileNo = HttpContext.Session.GetString("MobileNo");
        Debug.WriteLine("Account RechargeBalance()");
        try {
           bool success= await _telecomContext.RechargeBalance(MobileNo, paymentAmount, paymentMethod);
            if (success)
            {
                TempData["SuccessMessage"] = $"Recharged successfully for number: {MobileNo}";
            }
            else
            {
                TempData["ErrorMessage"] = "Incorrect data enetered";
            }
        }
        catch ( Exception e)
        {
            Debug.WriteLine("couldn't recharge balance for input account");
            Debug.WriteLine(e.Message);
            TempData["ErrorMessage"] = "couldn't recharge, try again later";
        }
        return View("RechargeBalanceView");

    }

}
