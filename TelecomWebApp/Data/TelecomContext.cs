using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using TelecomWebApp.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;

public class TelecomContext : DbContext
{
    public TelecomContext(DbContextOptions<TelecomContext> options)
        : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consumption>().HasNoKey();
        modelBuilder.Entity<NotSubbed>().HasNoKey();
        modelBuilder.Entity<Service_plan>().HasNoKey(); 
        modelBuilder.Entity<UsagePlan>().HasNoKey();
        modelBuilder.Entity<CustomerProfileActiveAccount>().HasNoKey();
        modelBuilder.Entity<PhysicalStoreVoucherDetail>().HasNoKey();
        modelBuilder.Entity<ResolvedTicketDetail>().HasNoKey();
        modelBuilder.Entity<CashbackTransactions>().HasNoKey();
        modelBuilder.Entity<CustomerWallet>().HasNoKey();
        modelBuilder.Entity<CustomerProfileActiveAccount>().HasNoKey();
        modelBuilder.Entity<PhysicalStoreVoucherDetail>().HasNoKey();
        modelBuilder.Entity<ResolvedTicketDetail>().HasNoKey();
        modelBuilder.Entity<E_shopVoucher>().HasNoKey();
        modelBuilder.Entity<AccountPayment>().HasNoKey();
        modelBuilder.Entity<Num_of_cashback>().HasNoKey();
        modelBuilder.Entity<CustomerAccountWithPlanDetail>().HasNoKey();
        modelBuilder.Entity<CustomerAccountsByPlanDate>().HasNoKey();
        modelBuilder.Entity<AccountUsagePlan>().HasNoKey();
        modelBuilder.Entity<RemoveBenefit>().HasNoKey();
        modelBuilder.Entity<SMSOffer>().HasNoKey();
        modelBuilder.Entity<PaymentPointsResults>().HasNoKey();
        modelBuilder.Entity<HighestValueVoucher>().HasNoKey();
        modelBuilder.Entity<Value>().HasNoKey();
        modelBuilder.Entity<UsageCurrMonth>().HasNoKey();
        modelBuilder.Entity<Payment>().HasNoKey();
    }


    public DbSet<UsageCurrMonth> UsageCurrMonths { get; set; }
    public DbSet<Service_plan> ServicePlans { get; set; }
    public DbSet<ResolvedTicketDetail> ResolvedTicketDetailsView { get; set; }
    public DbSet<HighestValueVoucher> vouchers { get; set; }
    public DbSet<Value> values { get; set; }
    public DbSet<Consumption> Consumption { get; set; }
    public DbSet<NotSubbed> ServicePlansNotSubbed { get; set; }
    public DbSet<CustomerWallet> CustomerWallets { get; set; }
    public DbSet<E_shopVoucher> E_ShopVouchers { get; set; }
    public DbSet<AccountPayment> AccountPayments { get; set; }
    public DbSet<Num_of_cashback> Num_Of_Cashbacks { get; set; }
    public DbSet<PaymentPointsResults> PaymentPointsResults { get; set; }
    public DbSet<CashbackTransactions> CashbackTransactions { get; set; }
    public DbSet<CustomerProfileActiveAccount> CustomerProfileActiveAccounts { get; set; }

    public DbSet<PhysicalStoreVoucherDetail> PhysicalStoreVoucherDetails { get; set; }

    public DbSet<ResolvedTicketDetail> ResolvedTicketDetails { get; set; }

    public DbSet<CustomerAccountWithPlanDetail> CustomerAccountWithPlanDetails { get; set; }

    public DbSet<CustomerAccountsByPlanDate> CustomerAccountsByPlanDateView { get; set; }

    public DbSet<AccountUsagePlan> AccountUsagePlans { get; set; }

    public DbSet<SMSOffer> SMSOffers { get; set; }
    public DbSet<Payment> payments { get; set; }
    public DbSet<Benefit> benefits { get; set; }

    private bool IsInvalidMobileNo(string mobileNo)
    {
        Debug.WriteLine("TelecomContext IsInvalidMobileNo()");
        int v = this.Database.SqlQuery<int>($"SELECT CASE WHEN EXISTS (SELECT 1 FROM Customer_Account WHERE MobileNo = {mobileNo}) THEN 1 ELSE 0 END AS Value").FirstOrDefault();
        return v == 0;
    }
    private bool IsInvalidPlanID(int  Plan_ID)
    {
        Debug.WriteLine("TelecomContext IsInvalidPlanID()");
        int v = this.Database.SqlQuery<int>($"SELECT CASE WHEN EXISTS (SELECT 1 FROM Service_Plan WHERE planID = {Plan_ID}) THEN 1 ELSE 0 END AS Value").FirstOrDefault();
        return v == 0;
    }

    private bool IsInvalidPaymentMethod(string payment_method)
    {
        Debug.WriteLine($"TelecomContext IsInvalidPaymentMethod({payment_method})");
        Debug.WriteLine(payment_method.Trim().ToLower());
        return payment_method.Trim().ToLower() != "cash" && payment_method.Trim().ToLower() != "credit";
    }

    private bool IsInvalidPaymentID(int paymentID)
    {

        Debug.WriteLine($"TelecomContext IsInvalidPaymentID({paymentID})");
        int v = this.Database.SqlQuery<int>
        ($"SELECT Count(1) as Value FROM Payment WHERE paymentID = {paymentID}").Single();
        Debug.WriteLine($"   val{v}");
        return v == 0;
    }
    private bool IsInvalidbenefitID(int benefitID)
    {
        Debug.WriteLine("TelecomContext IsInvalidbenefitID()");
        int v = this.Database.SqlQuery<int>($"SELECT CASE WHEN EXISTS (SELECT 1 FROM Benefits WHERE benefitID = {benefitID}) THEN 1 ELSE 0 END AS Value").FirstOrDefault();
        return v == 0;
    }
    private int GetNationalIDfromMobileNo(string mobileNo)
    {
        Debug.WriteLine("TelecomContext GetNationalIDfromMobileNo()");
        var nationalID = this.values.FromSqlInterpolated<Value>($"SELECT top 1 cp.nationalID AS Value FROM CUSTOMER_ACCOUNT cp WHERE cp.mobileNo = {mobileNo} ")
            .FirstOrDefault().value;

        return nationalID;
    }
    public async Task<List<UsageCurrMonth>> GetUsagePlanCurrentMonthAsync(string mobileNo)
    {
        Debug.WriteLine("TelecomContext GetUsagePlanCurrentMonthAsync()");
        return await UsageCurrMonths
            .FromSqlInterpolated($"SELECT * FROM dbo.Usage_Plan_CurrentMonth({mobileNo})")
            .ToListAsync();
    }
   
    public int GetRemainingPlanAmount(string mobileNo, string planName)
    {
        Debug.WriteLine("TelecomContext GetRemainingPlanAmount()");
        var result = this.Database.SqlQuery<int>(
            $"SELECT dbo.Remaining_plan_amount({mobileNo}, {planName})").FirstOrDefault();

        return result;
    }
    
	public int GetExtraPlanAmount(string mobileNo, String planName)
    {
        Debug.WriteLine("TelecomContext GetExtraPlanAmount()");
        var result = this.Database
            .SqlQuery<int>($"SELECT dbo.Extra_plan_amount({mobileNo}, {planName})")
            .FirstOrDefault();

        return result;
    }

    public async Task<HighestValueVoucher?> GetHighestValueVoucher(string mobileNo)
    {
        Debug.WriteLine("TelecomContext GetHighestValueVoucher()");
        var result = await this.vouchers
            .FromSqlInterpolated($"EXECUTE Account_Highest_Voucher @mobile_num = {mobileNo}")
            .ToListAsync();

        return result.FirstOrDefault();
    }

    public int GetUnresolvedTickets(String mobileNo)
    {
        Debug.WriteLine("TelecomContext GetUnresolvedTickets()");
        int nationalID = this.GetNationalIDfromMobileNo(mobileNo);
          Debug.WriteLine($"nationalID in GetUnresolvedTickets: {nationalID} " );
        var result = this.Database.SqlQuery<int>($"EXEC [Ticket_Account_Customer] @NID = {nationalID}").ToList();
        Debug.WriteLine($"no of unresolved tickets: {result} ");
        return result.First();
    }

    public async Task<List<Service_plan>> GetLast5MonthsServicePlans(String mobileNo)
    {
        Debug.WriteLine("TelecomContext GetLast5MonthsServicePlans()");
        var sp = await ServicePlans.FromSqlInterpolated($"SELECT * FROM Subscribed_plans_5_Months({mobileNo})").ToListAsync();
        foreach (var plan in sp)
        {
            Debug.WriteLine($"  PlanID: {plan.planID}, Name: {plan.name}, Price: {plan.price}, " +
                          $"SMS Offered: {plan.SMS_offered}, Minutes Offered: {plan.minutes_offered}, " +
                          $"Data Offered: {plan.data_offered}, Description: {plan.description}");
        }
        return sp;

    }
    public async Task<bool> RenewSubscription(String mobileNo, decimal amount, String payment_method, int plan_id) {

        
        Debug.WriteLine("TelecomContext RenewSubscription()");

        if (this.IsInvalidMobileNo(mobileNo)) 
            return false; 
        Debug.WriteLine("mobileNo exists for renewSubscription request" );

        
        if (this.IsInvalidPlanID(plan_id))
           return false; 
        Debug.WriteLine("planId exists for renewSubscription request");

        if (amount == 0) 
            return false;

        if (this.IsInvalidPaymentMethod(payment_method))
        {
            Debug.WriteLine($"invalid payment method: {payment_method}");
            return false;
        }
        Debug.WriteLine("valid payment method");


        await this.Database.ExecuteSqlInterpolatedAsync($"EXEC Initiate_plan_payment @mobile_num = {mobileNo},@amount = {amount},@payment_method ={payment_method},@plan_id = {plan_id}");
        Debug.WriteLine("renew subscription executed with no issues");
        return true;
    }

    public async Task<decimal> Payment_wallet_cashback(String mobileNo, int paymentID, int benefitID) {
        Debug.WriteLine($"TelecomContext Payment_wallet_cashback({mobileNo},{paymentID},{paymentID})");
        if (IsInvalidMobileNo(mobileNo))
        { return -1; }
        Debug.WriteLine("mobileNo exists for renewSubscription request");
        if (IsInvalidPaymentID(paymentID))
        { return -1; }
        Debug.WriteLine("payment ID exists for renewSubscription request");
        if (IsInvalidbenefitID(benefitID))
        { return -1; }
        Debug.WriteLine("benefit ID exists for renewSubscription request");

        await this.Database.ExecuteSqlInterpolatedAsync($"EXEC Payment_wallet_cashback @mobile_num = {mobileNo},@payment_id = {paymentID},@benefit_id = {benefitID}");
        return 0.1m * await this.Database.SqlQuery<decimal>($"select  p.amount AS Value  from Payment p where p.paymentID = {paymentID} and p.status = 'successful'").SingleAsync();

    }

    public async Task<List<Service_plan>> GetServicePlans()
    {
        return await ServicePlans
            .FromSqlInterpolated($"SELECT * FROM dbo.allServicePlans")
            .ToListAsync();
    }
    
    public async Task<List<Consumption>> GetConsumption(string planName, DateTime startDate, DateTime endDate)
    {
        return await Consumption
            .FromSqlInterpolated($"SELECT dbo.Consumption({planName}, {startDate}, {endDate})")
            .ToListAsync();
    }

    public async Task<List<NotSubbed>> GetServicePlansNotSubbed(string mobileNo)
    {
        return await ServicePlansNotSubbed
            .FromSqlInterpolated($"EXEC dbo.Unsubscribed_Plans @mobile_num = {mobileNo}")
            .ToListAsync();
    }

    public async Task<List<CashbackTransactions>> GetCashbackTransactions(String mobileNo)
    {
        int nationalID = this.GetNationalIDfromMobileNo(mobileNo);
        return await CashbackTransactions
            .FromSqlInterpolated($"SELECT * FROM dbo.Cashback_Wallet_Customer({nationalID})")
            .ToListAsync();
    }

    




    public bool login(String mobileNo, string password)
    {
        
        return this.Database.SqlQuery<bool>($"SELECT dbo.AccountLoginValidation({mobileNo},{password}) AS Value").FirstOrDefault();
    }

    public async Task<List<SMSOffer>> GetSMSOffersAsync(string mobileNo)
    {
        return await SMSOffers
            .FromSqlInterpolated($" SELECT * FROM dbo.Account_SMS_Offers({mobileNo})")
            .ToListAsync();
    }
    public async Task<List<Benefit>> GetAllActiveBenefits() {
        Debug.WriteLine("telecom GetAllActiveBenefits()");
        return await this.benefits.FromSqlRaw("SELECT * FROM allBenefits").ToListAsync();   
    }
    public async Task<List<AccountUsagePlan>> GetAccountUsagePlanAsync(string mobileNum, DateTime startDate)
    {
        return await AccountUsagePlans
            .FromSqlInterpolated($"SELECT * FROM dbo.Account_Usage_Plan({mobileNum}, {startDate})")
            .ToListAsync();
    }

    public async Task<List<CustomerAccountsByPlanDate>> GetCustomerAccountsByPlanDateAsync(DateTime subscriptionDate, int planId)
    {
        return await CustomerAccountsByPlanDateView
            .FromSqlInterpolated($"SELECT * FROM dbo.Account_Plan_date({subscriptionDate}, {planId})")
            .ToListAsync();
    }

    public async Task RemoveBenefitsAsync(string mobileNo, int planId)
    {
        await this.Database.ExecuteSqlRawAsync(
            "EXEC dbo.Benefits_Account @mobileNo, @planId",
            new SqlParameter("@mobileNo", mobileNo),
            new SqlParameter("@planId", planId)
        );
    }

    public async Task<List<Payment>> GetTopTenPayments(string mobileNo)
    {
        Debug.WriteLine("TelecomContext GetTopTenPayments()");
        return await this.payments.FromSql<Payment>($"EXEC Top_Successful_Payments @mobile_num = {mobileNo}").ToListAsync();
    }

    public DbSet<TelecomWebApp.Models.shop> shop { get; set; }



}
