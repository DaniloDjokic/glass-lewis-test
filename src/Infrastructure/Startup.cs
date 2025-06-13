using Application.Interfaces;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql("Host=localhost;Database=company-api-db;Username=myappuser;Password=admin"));

        services.AddTransient<ICompanyRepository, CompanyRepository>();
    }
}
