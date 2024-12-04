using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using TelecomWebApp.Models;

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
    }





    public async Task<List<UsagePlan>> GetUsagePlanCurrentMonthAsync(string mobileNo = "01012345678")
    {
        return await UsagePlans
            .FromSqlInterpolated($"SELECT * FROM dbo.Usage_Plan_CurrentMonth({mobileNo})")
            .ToListAsync();
    }
    public DbSet<UsagePlan> UsagePlans { get; set; }
    public DbSet<Service_plan> ServicePlans { get; set; }
    public DbSet<ResolvedTicketDetail> ResolvedTicketDetailsView { get; set; }


    public int GetRemainingPlanAmount(string mobileNo, string planName)
    {
        
        var result = this.Database.SqlQuery<int>(
            $"SELECT dbo.Remaining_plan_amount({mobileNo}, {planName})").FirstOrDefault();

        return result;
    }

	public int GetExtraPlanAmount(string mobileNo, string planName)
    {
        var result = this.Database
            .SqlQuery<int>($"SELECT dbo.Extra_plan_amount({mobileNo}, {planName})")
            .FirstOrDefault();

        return result;
    }
    //temp hardcoded
    public async Task<int> GetHighestValueVoucher(string mobileNo = "01012345678")
    {
        var result = await this.Database
        .SqlQuery<int>($"EXECUTE Account_Highest_Voucher @mobile_num = {mobileNo}")
        .FirstOrDefaultAsync();
        return result;
    }

    //temp hardcoded
	public int GetUnresolvedTickets(String mobileNo= "01012345678")
    {

        

        int nationalID = this.Database
            .SqlQuery<int>($"SELECT cp.nationalID AS Value FROM CUSTOMER_ACCOUNT cp WHERE cp.mobileNo = {mobileNo} ")
            .FirstOrDefault();
          Debug.WriteLine($"nationalID in GetUnresolvedTickets: {nationalID} " );
         var result = this.Database
            .SqlQuery<int>($"EXEC Ticket_Account_Customer @NID = {nationalID};")
            .FirstOrDefault(); 
        return result;
    }
    //temp hardcoded
    public async Task<List<Service_plan>> GetLast5MonthsServicePlans(String mobileNo = "01012345678")
    {
        var sp = await ServicePlans.FromSqlInterpolated($"SELECT * FROM Subscribed_plans_5_Months({mobileNo})").ToListAsync();
        foreach (var plan in sp)
        {
            Debug.WriteLine($"PlanID: {plan.planID}, Name: {plan.name}, Price: {plan.price}, " +
                          $"SMS Offered: {plan.SMS_offered}, Minutes Offered: {plan.minutes_offered}, " +
                          $"Data Offered: {plan.data_offered}, Description: {plan.description}");
        }
        return sp;

    }
    public async Task<bool> RenewSubscription(String mobileNo, decimal amount, String payment_method, int plan_id) {
        Debug.WriteLine("renewing subscription");
        if (0==await this.Database.ExecuteSqlInterpolatedAsync($"SELECT CASE WHEN EXISTS (SELECT 1 FROM Customer_Account WHERE MobileNo = {mobileNo}) THEN 1 ELSE 0 END AS MobileExists")) 
            { return false; }
        Debug.WriteLine("mobileNo exists for renewSubscription request" );
        if (0== await this.Database.ExecuteSqlInterpolatedAsync($"SELECT CASE WHEN EXISTS (SELECT 1 FROM Service_Plan WHERE planID = {plan_id}) THEN 1 ELSE 0 END AS MobileExists"))
        { return false; }
        Debug.WriteLine("planId exists for renewSubscription request");
        await this.Database.ExecuteSqlInterpolatedAsync($"EXEC Initiate_plan_payment @mobile_num = {mobileNo},@amount = {amount},@payment_method ={payment_method},@plan_id = {plan_id}");
        Debug.WriteLine("pproc executed with no issues");
        return true;
    }

    public async Task<double> Payment_wallet_cashback(String mobileNo, int paymentID, int benefitID) {
        if (0 == await this.Database.ExecuteSqlInterpolatedAsync($"SELECT CASE WHEN EXISTS (SELECT 1 FROM Customer_Account WHERE MobileNo = {mobileNo}) THEN 1 ELSE 0 END AS MobileExists"))
        { return -1; }
        Debug.WriteLine("mobileNo exists for renewSubscription request");
        if (0 == await this.Database.ExecuteSqlInterpolatedAsync($"SELECT CASE WHEN EXISTS (SELECT 1 FROM Payment WHERE paymentID = {paymentID}) THEN 1 ELSE 0 END AS planIDExists"))
        { return -1; }
        if (0 == await this.Database.ExecuteSqlInterpolatedAsync($"SELECT CASE WHEN EXISTS (SELECT 1 FROM Benefits WHERE benefitID = {benefitID}) THEN 1 ELSE 0 END AS benefitIDExists"))
        { return -1; }

        await this.Database.ExecuteSqlInterpolatedAsync($"EXEC Payment_wallet_cashback @mobile_num = {mobileNo},@payment_id = {paymentID},@benefit_id = {benefitID}");
        return 0.1 * await this.Database.ExecuteSqlInterpolatedAsync($"select  p.amount  from Payment where p.paymentID = {paymentID} and p.status = 'successful'");


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
    public DbSet<Consumption> Consumption { get; set; }
    public async Task<List<NotSubbed>> GetServicePlansNotSubbed(string mobileNo)
    {
        return await ServicePlansNotSubbed
            .FromSqlInterpolated($"SELECT * FROM dbo.Unsubscribed_Plans({mobileNo})")
            .ToListAsync();
    }
    public DbSet<NotSubbed> ServicePlansNotSubbed { get; set; }
    public DbSet<CustomerWallet> CustomerWallets { get; set; }
    public DbSet<E_shopVoucher> E_ShopVouchers { get; set; }
    public DbSet<AccountPayment> AccountPayments { get; set; }
    public DbSet<Num_of_cashback> Num_Of_Cashbacks { get; set; }
    public DbSet<PaymentPointsResults> PaymentPointsResults { get; set; }
    public async Task<List<CashbackTransactions>> GetCashbackTransactions(int nationalID)
    {
        return await CashbackTransactions
            .FromSqlInterpolated($"SELECT * FROM dbo.Cashback_Wallet_Customer({nationalID})")
            .ToListAsync();
    }
    public DbSet<CashbackTransactions> CashbackTransactions { get; set; }

    public bool login(String mobileNo, string password)
    {
        
        return this.Database.SqlQuery<bool>($"SELECT dbo.AccountLoginValidation({mobileNo},{password}) AS Value").FirstOrDefault();
        
        
    }

    public DbSet<CustomerProfileActiveAccount> CustomerProfileActiveAccounts { get; set; }

    public DbSet<PhysicalStoreVoucherDetail> PhysicalStoreVoucherDetails { get; set; }

    public DbSet<ResolvedTicketDetail> ResolvedTicketDetails { get; set; }

    public DbSet<CustomerAccountWithPlanDetail> CustomerAccountWithPlanDetails { get; set; }

    public DbSet<CustomerAccountsByPlanDate> CustomerAccountsByPlanDateView { get; set; }

    public DbSet<AccountUsagePlan> AccountUsagePlans { get; set; }

    public DbSet<SMSOffer> SMSOffers { get; set; }

    public async Task<List<SMSOffer>> GetSMSOffersAsync(string mobileNo)
    {
        return await SMSOffers
            .FromSqlInterpolated($" SELECT * FROM dbo.Account_SMS_Offers({mobileNo})")
            .ToListAsync();
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

   
}
