using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public required DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>()
        .HasData(
            new Company
            {
                Id = 1,
                Name = "Apple Inc.",
                Exchange = "NASDAQ",
                Ticker = "AAPL",
                Isin = "US0378331005",
                WebsiteUrl = "https://www.apple.com"
            },
            new Company
            {
                Id = 2,
                Name = "Microsoft Corporation",
                Exchange = "NASDAQ",
                Ticker = "MSFT",
                Isin = "US5949181045",
                WebsiteUrl = "https://www.microsoft.com"
            },
            new Company
            {
                Id = 3,
                Name = "Alphabet Inc.",
                Exchange = "NASDAQ",
                Ticker = "GOOGL",
                Isin = "US02079K3059",
                WebsiteUrl = "https://www.google.com"
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}
