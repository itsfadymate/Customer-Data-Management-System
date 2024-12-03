using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using TelecomWebApp.Models;

public class TelecomContext : DbContext
{
    public TelecomContext(DbContextOptions<TelecomContext> options)
        : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consumption>().HasNoKey();
        modelBuilder.Entity<NotSubbed>().HasNoKey();
        modelBuilder.Entity<ServicePlan>().HasNoKey(); 
        modelBuilder.Entity<UsagePlan>().HasNoKey();
        modelBuilder.Entity<CustomerWallet>().HasNoKey();
        modelBuilder.Entity<CustomerProfileActiveAccount>().HasNoKey();
        modelBuilder.Entity<PhysicalStoreVoucherDetails>().HasNoKey();
        modelBuilder.Entity<ResolvedTicketDetails>().HasNoKey();
        modelBuilder.Entity<E_shopVoucher>().HasNoKey();
        modelBuilder.Entity<AccountPayment>().HasNoKey();
        modelBuilder.Entity<Num_of_cashback>().HasNoKey();
        modelBuilder.Entity<CustomerAccountWithPlan>().HasNoKey();
        modelBuilder.Entity<CustomerAccountsByPlanDate>().HasNoKey();
    }


    public async Task<List<UsagePlan>> GetUsagePlanCurrentMonthAsync(string mobileNo)
    {
        return await UsagePlans
            .FromSqlInterpolated($"SELECT * FROM dbo.Usage_Plan_CurrentMonth({mobileNo})")
            .ToListAsync();
    }
    public DbSet<UsagePlan> UsagePlans { get; set; }

    
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

	 public int GetHighestValueVoucher(string mobileNo)
    {
        var result = this.Database
            .SqlQuery<int>($"EXEC Account_Highest_Voucher @MobileNo = {mobileNo}") 
            .FirstOrDefault();

        return result;
    }
	public int GetUnresolvedTickets(String mobileNo)
    {

	int nationalID = this.Database
            .SqlQuery<int>($"SELECT cp.nationalID FROM CUSTOMER_ACCOUNT cp WHERE cp.mobileNo = {mobileNo} ")
            .FirstOrDefault();

         var result = this.Database
            .SqlQuery<int>($"EXEC Ticket_Account_Customer @NationalID = {nationalID }")
            .FirstOrDefault(); 
        return result;
    }

    public async Task<List<ServicePlan>> GetServicePlans()
    {
        return await ServicePlans
            .FromSqlInterpolated($"SELECT * FROM dbo.allServicePlans")
            .ToListAsync();
    }
    public DbSet<ServicePlan> ServicePlans { get; set; }
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
    public DbSet<ResolvedTicketDetails> ResolvedTicketDetailsView { get; set; }

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
    public DbSet<CustomerAccountsByPlanDate> CustomerAccountsByPlanDateView { get; set; }

    public async Task<List<CustomerAccountsByPlanDate>> GetCustomerAccountsByPlanDateAsync(DateTime subscriptionDate, int planId)
    {
        return await CustomerAccountsByPlanDateView
            .FromSqlInterpolated($"SELECT * FROM dbo.Account_Plan_date({subscriptionDate}, {planId})")
            .ToListAsync();
    }
















}
