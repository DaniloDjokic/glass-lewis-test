using System.Text.Json;
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
}
