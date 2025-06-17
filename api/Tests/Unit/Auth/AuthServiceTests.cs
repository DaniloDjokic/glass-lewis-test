using System.Text.Json;
using Application.DTOs;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Tests.Unit;

[Trait("Category", "Unit")]
public class AuthServiceTests
{
    private readonly IConfigurationRoot _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthServiceTests()
    {
        _logger = new Logger<AuthService>(new LoggerFactory());
        _configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "IdentityServer:Authority", "https://identityserver.test" },
            { "IdentityServer:GrantType", "password" },
            { "IdentityServer:ClientId", "test_client" },
            { "IdentityServer:ClientSecret", "test_secret" },
            { "IdentityServer:Scope", "api1" }
        }).Build();
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsSuccessResponse()
    {
        var httpClient = new HttpClient(new HttpMessageHandlerStub(new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new StringContent("{\"access_token\":\"valid_token\"}")
        }));

        var httpClientFactory = new HttpClientFactoryStub(httpClient);
        var authService = new AuthService(httpClientFactory, _configuration, _logger);

        var loginRequest = new UserLoginRequestDTO("testuser", "testpassword");

        var response = await authService.LoginAsync(loginRequest);

        Assert.True(response.Success);
        Assert.Equal("valid_token", response.Token);
        Assert.Equal("", response.ErrorMessage);
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ReturnsErrorResponse()
    {
        var httpClient = new HttpClient(new HttpMessageHandlerStub(new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.Unauthorized,
            Content = new StringContent("{\"error\":\"invalid_grant\"}")
        }));

        var httpClientFactory = new HttpClientFactoryStub(httpClient);
        var authService = new AuthService(httpClientFactory, _configuration, _logger);
        var loginRequest = new UserLoginRequestDTO("testuser", "wrongpassword");

        var response = await authService.LoginAsync(loginRequest);

        Assert.False(response.Success);
        Assert.Equal("", response.Token);
        Assert.NotEqual("", response.ErrorMessage);
    }

    [Fact]
    public async Task LoginAsync_MalformedResponse_ThrowsJsonException()
    {
        var httpClient = new HttpClient(new HttpMessageHandlerStub(new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new StringContent("malformed response")
        }));

        var httpClientFactory = new HttpClientFactoryStub(httpClient);
        var authService = new AuthService(httpClientFactory, _configuration, _logger);
        var loginRequest = new UserLoginRequestDTO("testuser", "testpassword");

        await Assert.ThrowsAsync<JsonException>(() => authService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task LoginAsync_EmptyResponse_ThrowsArgumentNullException()
    {
        var httpClient = new HttpClient(new HttpMessageHandlerStub(new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new StringContent("null")
        }));

        var httpClientFactory = new HttpClientFactoryStub(httpClient);
        var authService = new AuthService(httpClientFactory, _configuration, _logger);
        var loginRequest = new UserLoginRequestDTO("testuser", "testpassword");

        await Assert.ThrowsAsync<ArgumentNullException>(() => authService.LoginAsync(loginRequest));
    }
}
