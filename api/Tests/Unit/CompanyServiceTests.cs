namespace Tests.Unit;

using Application.DTOs;
using Application.Exceptions;
using Application.Services;

public class CompanyServiceTests
{
    [Fact]
    public async Task GetCompanies_ReturnsListOfCompanies()
    {
        var repository = new CompanyRepositoryStub();
        var expectedCompanies = repository.Companies;

        var service = new CompanyService(repository);

        var companies = await service.GetCompaniesAsync();

        Assert.NotNull(companies);
        Assert.NotEmpty(companies);

        Assert.Equal(expectedCompanies, companies);
    }

    [Fact]
    public async Task GetCompanyById_ValidId_ReturnsCompany()
    {
        var repository = new CompanyRepositoryStub();
        var expectedCompany = repository.Companies.FirstOrDefault(c => c.Id == 1);

        var service = new CompanyService(repository);

        var company = await service.GetCompanyByIdAsync(1);

        Assert.NotNull(company);
        Assert.Equal(expectedCompany, company);
    }

    [Fact]
    public async Task GetCompanyById_InvalidId_ReturnsNull()
    {
        var repository = new CompanyRepositoryStub();
        var service = new CompanyService(repository);

        var company = await service.GetCompanyByIdAsync(999); // Non-existent ID

        Assert.Null(company);
    }

    [Fact]
    public async Task GetCompanyByIsin_ValidIsin_ReturnsCompany()
    {
        var repository = new CompanyRepositoryStub();
        var isin = "US0378331005"; // ISIN for Apple Inc.
        var expectedCompany = repository.Companies.FirstOrDefault(c => c.Isin == isin);

        var service = new CompanyService(repository);

        var company = await service.GetCompanyByIsinAsync(isin);

        Assert.NotNull(company);
        Assert.Equal(expectedCompany, company);
    }

    [Fact]
    public async Task GetCompanyByIsin_InvalidIsin_ReturnsNull()
    {
        var repository = new CompanyRepositoryStub();
        var service = new CompanyService(repository);

        var company = await service.GetCompanyByIsinAsync("INVALIDISIN");

        Assert.Null(company);
    }

    [Fact]
    public async Task CreateCompany_ValidData_ReturnsNewCompanyId()
    {
        var repository = new CompanyRepositoryStub();
        var expectedId = repository.Companies.Max(c => c.Id) + 1; // Next ID for new company

        var service = new CompanyService(repository);

        var newCompanyDto = new CreateCompanyDTO(
            "New Company",
            "NYSE",
            "NEWC",
            "US1234567890",
            "https://www.newcompany.com"
        );

        var newCompanyId = await service.CreateCompanyAsync(newCompanyDto);

        Assert.True(newCompanyId == expectedId);
    }

    [Fact]
    public async Task CreateCompany_DuplicateIsin_ThrowsDuplicateIsinException()
    {
        var repository = new CompanyRepositoryStub();
        var service = new CompanyService(repository);

        var duplicateIsinDto = new CreateCompanyDTO(
            "Duplicate Company",
            "NASDAQ",
            "DUPC",
            "US0378331005", // ISIN for Apple Inc.
            "https://www.duplicatecompany.com"
        );

        await Assert.ThrowsAsync<DuplicateIsinException>(() => service.CreateCompanyAsync(duplicateIsinDto));
    }

    [Fact]
    public async Task CreateCompany_InvalidIsin_ThrowsInvalidIsinException()
    {
        var repository = new CompanyRepositoryStub();
        var service = new CompanyService(repository);

        var invalidIsinDto = new CreateCompanyDTO(
            "Invalid ISIN Company",
            "NASDAQ",
            "INVC",
            "12INVALID", // Invalid ISIN
            "https://www.invalidisincompany.com"
        );

        await Assert.ThrowsAsync<InvalidIsinException>(() => service.CreateCompanyAsync(invalidIsinDto));
    }

    [Fact]
    public async Task UpdateCompany_ValidData_UpdatesCompany()
    {
        var repository = new CompanyRepositoryStub();
        var service = new CompanyService(repository);

        var updateDto = new UpdateCompanyDTO(
            "Updated Apple Inc.",
            "NASDAQ",
            "AAPL",
            "US0378331005", // Same ISIN
            "https://www.apple.com/updated"
        );

        await service.UpdateCompanyAsync(1, updateDto);
        var updatedCompany = await service.GetCompanyByIdAsync(1);

        Assert.NotNull(updatedCompany);
        Assert.Equal("Updated Apple Inc.", updatedCompany.Name);
    }

    [Fact]
    public async Task UpdateCompany_NonExistentId_ThrowsCompanyNotFoundException()
    {
        var repository = new CompanyRepositoryStub();
        var service = new CompanyService(repository);

        var updateDto = new UpdateCompanyDTO(
            "Non-existent Company",
            "NYSE",
            "NONC",
            "US1234567890",
            "https://www.nonexistentcompany.com"
        );

        await Assert.ThrowsAsync<CompanyNotFoundException>(() => service.UpdateCompanyAsync(999, updateDto));
    }

    [Fact]
    public async Task UpdateCompany_UpdatingIsin_DuplicateIsin_ThrowsDuplicateIsinException()
    {
        var repository = new CompanyRepositoryStub();
        var service = new CompanyService(repository);

        var updateDto = new UpdateCompanyDTO(
            "Updated Microsoft Corporation",
            "NASDAQ",
            "MSFT",
            "US0378331005", // ISIN for Apple Inc.
            "https://www.microsoft.com/updated"
        );

        await Assert.ThrowsAsync<DuplicateIsinException>(() => service.UpdateCompanyAsync(2, updateDto));
    }

    [Fact]
    public async Task UpdateCompany_UpdatingIsin_InvalidIsin_ThrowsInvalidIsinException()
    {
        var repository = new CompanyRepositoryStub();
        var service = new CompanyService(repository);

        var updateDto = new UpdateCompanyDTO(
            "Updated Alphabet Inc.",
            "NASDAQ",
            "GOOGL",
            "12INVALID", // Invalid ISIN
            "https://www.google.com/updated"
        );

        await Assert.ThrowsAsync<InvalidIsinException>(() => service.UpdateCompanyAsync(3, updateDto));
    }
}
