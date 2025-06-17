using Application.Common;
using Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<AuthService> logger) : IAuthService
{
    public async Task<UserLoginResponseDTO> LoginAsync(UserLoginRequestDTO loginRequest)
    {
        logger.LogInformation("Attempting to log in user: {Username}", loginRequest.Username);

        var identityServerUrl = configuration["IdentityServer:Authority"] ?? throw new ArgumentNullException("IdentityServer URL is not configured.");

        var tokenUrl = $"{identityServerUrl}/connect/token";

        var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", configuration["IdentityServer:GrantType"] ?? throw new ArgumentNullException("Grant type is not configured.") },
                { "username", loginRequest.Username },
                { "password", loginRequest.Password },
                { "client_id", configuration["IdentityServer:ClientId"] ?? throw new ArgumentNullException("Client ID is not configured.") },
                { "client_secret", configuration["IdentityServer:ClientSecret"] ?? throw new ArgumentNullException("Client secret is not configured.") },
                { "scope", configuration["IdentityServer:Scope"] ?? throw new ArgumentNullException("Scope is not configured.") }
            })
        };

        var client = httpClientFactory.CreateClient("IdentityServerClient");
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = System.Text.Json.JsonSerializer.Deserialize<AuthServerSuccessResponse>(responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (loginResponse is null)
            {
                throw new ArgumentNullException("Login response is null. Please check the IdentityServer configuration.");
            }

            return new UserLoginResponseDTO(true, loginResponse.AccessToken, string.Empty);
        }
        else
        {
            var errContent = await response.Content.ReadAsStringAsync();
            var errResponse = System.Text.Json.JsonSerializer.Deserialize<AuthServerErrorResponse>(errContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            logger.LogError("Login failed for user {Username}. Status code: {StatusCode}, Error: {Error}, Error Description: {ErrorDescription}",
                loginRequest.Username, response.StatusCode, errResponse?.Error, errResponse?.ErrorDescription);

            return new UserLoginResponseDTO(false, "", $"Login failed: {errResponse?.ErrorDescription ?? "Unknown error"}");
        }
    }
}
