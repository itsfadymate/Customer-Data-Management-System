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

}
