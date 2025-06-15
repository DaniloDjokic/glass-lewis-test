using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceExtensions
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ICompanyService, CompanyService>();
    }
}
