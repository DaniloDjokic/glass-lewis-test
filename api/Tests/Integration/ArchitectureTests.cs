namespace Tests.Integration;

[Trait("Category", "Integration")]
[Trait("Category", "Architecture")]
public class ArchitectureTests
{
    [Fact]
    public void Domain_Should_Not_Have_Dependency_On_Other_Layers()
    {
        var domainAssembly = typeof(Domain.Entities.Company).Assembly;
        var referencedAssemblies = domainAssembly.GetReferencedAssemblies();

        Assert.DoesNotContain(referencedAssemblies, a =>
                (a?.Name?.Contains("Application") ?? false) ||
                (a?.Name?.Contains("Infrastructure") ?? false) ||
                (a?.Name?.Contains("CompanyAPI") ?? false));
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure_Or_Web()
    {
        var applicationAssembly = typeof(Application.Services.CompanyService).Assembly;
        var referencedAssemblies = applicationAssembly.GetReferencedAssemblies();

        Assert.DoesNotContain(referencedAssemblies, a =>
                (a?.Name?.Contains("Infrastructure") ?? false) ||
                (a?.Name?.Contains("CompanyAPI") ?? false));
    }

    [Fact]
    public void Infrastructure_Should_Depend_On_Domain_And_Application()
    {
        var infrastructureAssembly = typeof(Infrastructure.ApplicationDbContext).Assembly;
        var referencedAssemblies = infrastructureAssembly.GetReferencedAssemblies();

        Assert.Contains(referencedAssemblies, a => a?.Name?.Contains("Domain") ?? false);
        Assert.Contains(referencedAssemblies, a => a?.Name?.Contains("Application") ?? false);
    }

    [Fact]
    public void Web_Should_Depend_On_Application_And_Infrastructure()
    {
        var webAssembly = typeof(CompanyApi.Controllers.CompanyController).Assembly;
        var referencedAssemblies = webAssembly.GetReferencedAssemblies();

        Assert.Contains(referencedAssemblies, a => a?.Name?.Contains("Application") ?? false);
        Assert.Contains(referencedAssemblies, a => a?.Name?.Contains("Infrastructure") ?? false);
    }

    [Fact]
    public void Web_Should_Not_Depend_On_Domain()
    {
        var webAssembly = typeof(CompanyApi.Controllers.CompanyController).Assembly;
        var referencedAssemblies = webAssembly.GetReferencedAssemblies();

        Assert.DoesNotContain(referencedAssemblies, a => a?.Name?.Contains("Domain") ?? false);
    }
}
