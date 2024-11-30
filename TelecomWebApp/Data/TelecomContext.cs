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

    p
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
