using System.Net;
using System.Text.Json;
using Application.DTOs;
using Domain.Entities;

namespace Tests.Integration;

[Trait("Category", "e2e")]
[Trait("Category", "Integration")]
public class CompanyControllerTests : IClassFixture<CompanyApiTestFactory>
{
    private readonly CompanyApiTestFactory _factory;
    private readonly HttpClient _client;

    public CompanyControllerTests(CompanyApiTestFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCompanies_ReturnsOkWithCompanies()
    {
        var response = await _client.GetAsync("/api/companies");

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var companies = JsonSerializer.Deserialize<List<Company>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(companies);
        Assert.NotEmpty(companies);
    }

    [Fact]
    public async Task GetCompanyById_ReturnsOkWithCompany()
    {
        var response = await _client.GetAsync("/api/companies/1");

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var company = JsonSerializer.Deserialize<Company>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(company);
        Assert.Equal(1, company.Id);
    }

    [Fact]
    public async Task GetCompanyById_ReturnsNotFound_WhenCompanyDoesNotExist()
    {
        var response = await _client.GetAsync("/api/companies/999");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCompanyByIsin_ReturnsOkWithCompany()
    {
        var response = await _client.GetAsync("/api/companies/isin/US0378331005");

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var company = JsonSerializer.Deserialize<Company>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(company);
        Assert.Equal("US0378331005", company.Isin);
    }

    [Fact]
    public async Task GetCompanyByIsin_ReturnsNotFound_WhenCompanyDoesNotExist()
    {
        var response = await _client.GetAsync("/api/companies/isin/INVALIDISIN");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateCompany_ReturnsCreatedWithCompanyId()
    {
        var newCompany = new CreateCompanyDTO("Test Company", "US1234567890", "NASDAQ", "Test", "");

        var content = new StringContent(JsonSerializer.Serialize(newCompany), System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/companies", content);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var location = response.Headers.Location;
        Assert.NotNull(location);

        var idStr = location.Segments.Last();
        Assert.True(int.TryParse(idStr, out var companyId));
        Assert.NotEqual(0, companyId);

        Assert.NotEqual(0, companyId);
    }

    [Fact]
    public async Task CreateCompany_DuplicateIsin_ReturnsConflict()
    {
        var existingCompany = _factory.GetDbContext().Companies.First();
        var newCompany = new CreateCompanyDTO("Duplicate Company", "NASDAQ", "Test", existingCompany.Isin, "");

        var content = new StringContent(JsonSerializer.Serialize(existingCompany), System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/companies", content);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task CreateCompany_InvalidIsin_ReturnsBadRequest()
    {
        var newCompany = new CreateCompanyDTO("Test", "Test", "Test", "12INVALID", "");

        var content = new StringContent(JsonSerializer.Serialize(newCompany), System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/companies", content);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCompany_ReturnsNoContent()
    {
        var existingCompany = _factory.GetDbContext().Companies.First();
        existingCompany.Name = "Updated Company Name";
        var dto = new UpdateCompanyDTO(existingCompany.Name, existingCompany.Exchange, existingCompany.Ticker, existingCompany.Isin, existingCompany.WebsiteUrl);

        var content = new StringContent(JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/api/companies/{existingCompany.Id}", content);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCompany_InvalidId_ReturnsNotFound()
    {
        var dto = new UpdateCompanyDTO("Updated Company", "NASDAQ", "TEST", "US1234567890", "");

        var content = new StringContent(JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PutAsync("/api/companies/999", content);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCompany_InvalidIsin_ReturnsBadRequest()
    {
        var existingCompany = _factory.GetDbContext().Companies.First();
        var dto = new UpdateCompanyDTO("Updated Company", "NASDAQ", "TEST", "12INVALID", "");

        var content = new StringContent(JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/api/companies/{existingCompany.Id}", content);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCompany_UpdatingIsinWithDuplicate_ReturnsConflict()
    {
        var existingCompany = _factory.GetDbContext().Companies.First();
        var duplicateCompany = _factory.GetDbContext().Companies.Skip(1).First();
        var dto = new UpdateCompanyDTO("Updated Company", "NASDAQ", "TEST", duplicateCompany.Isin, "");

        var content = new StringContent(JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/api/companies/{existingCompany.Id}", content);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
