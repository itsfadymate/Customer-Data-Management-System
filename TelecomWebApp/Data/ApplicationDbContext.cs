using Microsoft.EntityFrameworkCore;

namespace TelecomWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSet properties for your tables
        //public DbSet<Wallet> Wallets { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }
        //public DbSet<Eshop> Eshops { get; set; }
        // Add other DbSet properties as needed
    }
}
