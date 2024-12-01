using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using TelecomWebApp.Models;

public class TelecomContext : DbContext
{
    public TelecomContext(DbContextOptions<TelecomContext> options)
        : base(options) { }

    public async Task<List<UsagePlan>> GetUsagePlanCurrentMonthAsync(string mobileNo)
    {
        return await UsagePlans
            .FromSqlInterpolated($"SELECT * FROM dbo.Usage_Plan_CurrentMonth({mobileNo})")
            .ToListAsync();
    }
    public DbSet<UsagePlan> UsagePlans { get; set; }

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
            .FromSqlInterpolated($"SELECT * FROM dbo.Consumption({planName}, {startDate}, {endDate})")
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

	 public int GetHighestVoucher(string mobileNo)
    {
        var result = this.Database
            .SqlQuery<int>("EXEC Account_Highest_Voucher @MobileNo = {0}", mobileNo)
            .FirstOrDefault();

        return result;
    }
	public int GetUnresolvedTicketsByCustomer(int nationalId)
    {
         var result = this.Database
            .SqlQuery<int>("EXEC Ticket_Account_Customer @NationalID = {0}", nationalId)
            .FirstOrDefault(); 
        return result;
    }



}
