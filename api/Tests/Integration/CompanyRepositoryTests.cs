using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Infrastructure;

namespace Tests.Integration;

public class CompanyRepositoryTests : IntegrationTestBase<ApplicationDbContext>
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyRepositoryTests()
    {
        _companyRepository = new CompanyRepository(DbContext);
    }

    protected override void SeedDatabase(ApplicationDbContext context)
    {
        var companies = new List<Company>(){
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
        };

        context.Companies.AddRange(companies);
        context.SaveChanges();
    }

    [Fact]
    public async Task GetById_ExistingCompany_ReturnsCompany()
    {
        var existingCompany = DbContext.Companies.First();

        var result = await _companyRepository.GetCompanyByIdAsync(existingCompany.Id);

        Assert.NotNull(result);
        Assert.Equal(existingCompany.Id, result.Id);
    }

    [Fact]
    public async Task GetById_NonExistentCompany_ReturnsNull()
    {
        var result = await _companyRepository.GetCompanyByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllCompanies_ReturnsAllCompanies()
    {
        var result = await _companyRepository.GetAllCompaniesAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task AddCompany_ValidCompany_AddsCompany()
    {
        var newCompany = new Application.DTOs.CreateCompanyDTO(
            Name: "Tesla Inc.",
            Exchange: "NASDAQ",
            Ticker: "TSLA",
            Isin: "US88160R1014",
            WebsiteUrl: "https://www.tesla.com"
        );

        var id = await _companyRepository.CreateCompanyAsync(newCompany);
        await DbContext.SaveChangesAsync();

        var addedCompany = await _companyRepository.GetCompanyByIdAsync(id);
    }
}
