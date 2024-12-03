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
        modelBuilder.Entity<PhysicalStoreVoucherDetails>().HasNoKey();
        modelBuilder.Entity<ResolvedTicketDetails>().HasNoKey(); 
    }





    public async Task<List<UsagePlan>> GetUsagePlanCurrentMonthAsync(string mobileNo = "01012345678")
    {
        return await UsagePlans
            .FromSqlInterpolated($"SELECT * FROM dbo.Usage_Plan_CurrentMonth({mobileNo})")
            .ToListAsync();
    }
    public DbSet<UsagePlan> UsagePlans { get; set; }
    public DbSet<Service_plan> ServicePlans { get; set; }
    public DbSet<ResolvedTicketDetails> ResolvedTicketDetailsView { get; set; }


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

	public int GetHighestValueVoucher(string mobileNo = "01012345678")
    {
        var result = this.Database
            .SqlQuery<int>($"EXEC Account_Highest_Voucher @MobileNo = {mobileNo}") 
            .FirstOrDefault();

        return result;
    }

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

    public async Task<List<Service_plan>> GetLast5MonthsServicePlans(String mobileNo = "01012345678")
    {
        var sp = await ServicePlans.FromSqlInterpolated($"SELECT dbo.Subscribed_plans_5_Months({mobileNo})").ToListAsync();
        foreach (var plan in sp)
        {
            Debug.WriteLine($"PlanID: {plan.planID}, Name: {plan.name}, Price: {plan.price}, " +
                          $"SMS Offered: {plan.SMS_offered}, Minutes Offered: {plan.minutes_offered}, " +
                          $"Data Offered: {plan.data_offered}, Description: {plan.description}");
        }
        return sp;

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

    public bool login(String mobileNo, string password)
    {
        
        return this.Database.SqlQuery<bool>($"SELECT dbo.AccountLoginValidation({mobileNo},{password}) AS Value").FirstOrDefault();
        
        
    }

    public DbSet<CustomerProfileActiveAccount> CustomerProfileActiveAccounts { get; set; }

    public async Task<List<CustomerProfileActiveAccount>> GetCustomerProfilesWithActiveAccountsAsync()
    {
        return await CustomerProfileActiveAccounts
            .FromSqlInterpolated($"EXEC allCustomerAccounts")
            .ToListAsync();
    }

    public DbSet<PhysicalStoreVoucherDetails> PhysicalStoreVoucherDetailsView { get; set; }

    public async Task<List<PhysicalStoreVoucherDetails>> GetPhysicalStoreVoucherDetailsAsync()
    {
        return await PhysicalStoreVoucherDetailsView
            .FromSqlInterpolated($"EXEC PhysicalStoreVouchers")
            .ToListAsync();
    }
  

    public async Task<List<ResolvedTicketDetails>> GetResolvedTicketsAsync()
    {
        return await ResolvedTicketDetailsView
            .FromSqlInterpolated($"EXEC allResolvedTickets")
            .ToListAsync();
    }
    public async Task<List<CustomerAccountWithPlan>> GetCustomerAccountsWithPlansAsync()
    {
        return await this.Database
            .SqlQuery<CustomerAccountWithPlan>($"EXEC Account_Plan")
            .ToListAsync();
    }
















}
