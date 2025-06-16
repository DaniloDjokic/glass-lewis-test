using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Tests.Integration;

public class CompanyApiTestFactory : WebApplicationFactory<Program>
{
    public string DatabaseName { get; } = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
            {
                var dbContextTypes = services
                    .Where(d => d.ServiceType.BaseType == typeof(DbContext) ||
                               d.ServiceType == typeof(DbContextOptions) ||
                               d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))
                    .ToList();

                foreach (var dbContextType in dbContextTypes)
                {
                    services.Remove(dbContextType);
                }

                var efServices = services
                    .Where(d => d.ServiceType.Assembly.FullName?.Contains("EntityFramework") == true)
                    .ToList();

                foreach (var efService in efServices)
                {
                    services.Remove(efService);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(DatabaseName);
                    options.EnableSensitiveDataLogging();
                });

                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                SeedDatabase(context);
            });

        builder.UseEnvironment("Testing");
    }

    // This method is not abstract due to xUnit limitations on abstract classes.
    protected virtual void SeedDatabase(ApplicationDbContext context) { }

    public ApplicationDbContext GetDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}
