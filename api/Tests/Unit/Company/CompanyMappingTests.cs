using Application.DTOs;
using Domain.Entities;

namespace Tests.Unit;

[Trait("Category", "Unit")]
public class CompanyMappingTests
{
    [Fact]
    public void CompanyDTO_FromEntity_MapsProperly()
    {
        var company = new Company
        {
            Id = 1,
            Name = "Test Company",
            Exchange = "NYSE",
            Ticker = "TEST",
            Isin = "US1234567890",
            WebsiteUrl = "https://www.testcompany.com"
        };

        var companyDto = CompanyDTO.FromEntity(company);

        Assert.Equal(company.Id, companyDto.Id);
        Assert.Equal(company.Name, companyDto.Name);
        Assert.Equal(company.Exchange, companyDto.Exchange);
        Assert.Equal(company.Ticker, companyDto.Ticker);
        Assert.Equal(company.Isin, companyDto.Isin);
        Assert.Equal(company.WebsiteUrl, companyDto.WebsiteUrl);
    }

    [Fact]
    public void CreateCompanyDTO_ToEntity_MapsProperly()
    {
        var createCompanyDto = new CreateCompanyDTO(
            "Test Company",
            "NYSE",
            "TEST",
            "US1234567890",
            "https://www.testcompany.com"
        );

        var company = CreateCompanyDTO.ToEntity(createCompanyDto);

        Assert.Equal(createCompanyDto.Name, company.Name);
        Assert.Equal(createCompanyDto.Exchange, company.Exchange);
        Assert.Equal(createCompanyDto.Ticker, company.Ticker);
        Assert.Equal(createCompanyDto.Isin, company.Isin);
        Assert.Equal(createCompanyDto.WebsiteUrl, company.WebsiteUrl);
    }

    [Fact]
    public void UpdateCompanyDto_MapCompany_MapsCompanyProperly()
    {
        var updateCompanyDto = new UpdateCompanyDTO(
            "Updated Company",
            "NASDAQ",
            "UPDT",
            "US0987654321",
            "https://www.updatedcompany.com"
        );

        var company = new Company
        {
            Name = "Updated Company",
            Exchange = "NASDAQ",
            Ticker = "UPDT",
            Isin = "US0987654321",
            WebsiteUrl = "https://www.updatedcompany.com"
        };

        updateCompanyDto.MapCompany(company);

        Assert.Equal(updateCompanyDto.Name, company.Name);
        Assert.Equal(updateCompanyDto.Exchange, company.Exchange);
        Assert.Equal(updateCompanyDto.Ticker, company.Ticker);
        Assert.Equal(updateCompanyDto.Isin, company.Isin);
        Assert.Equal(updateCompanyDto.WebsiteUrl, company.WebsiteUrl);
    }
}
