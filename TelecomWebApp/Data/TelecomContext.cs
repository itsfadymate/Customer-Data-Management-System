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

    public async Task<int> GetUnresolvedTickets(int nationalID)
    {
        var outputParam = new SqlParameter
        {
            ParameterName = "@out",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Output
        };

        await Database.ExecuteSqlInterpolatedAsync($@"
        EXEC proc Ticket_Account_Customer {nationalID}, @out OUTPUT
    ");

        return (int)outputParam.Value;
    }



}
