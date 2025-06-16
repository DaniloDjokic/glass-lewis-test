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

        var identityServerUrl = configuration["IdentityServer:Authority"] ?? throw new InvalidOperationException("IdentityServer URL is not configured.");

        var tokenUrl = $"{identityServerUrl}/connect/token";

        var request = new HttpRequestMessage(HttpMethod.Post, identityServerUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", configuration["IdentityServer:GrantType"] ?? throw new InvalidOperationException("Grant type is not configured.") },
                { "username", loginRequest.Username },
                { "password", loginRequest.Password },
                { "client_id", configuration["IdentityServer:ClientId"] ?? throw new InvalidOperationException("Client ID is not configured.") },
                { "client_secret", configuration["IdentityServer:ClientSecret"] ?? throw new InvalidOperationException("Client secret is not configured.") },
                { "scope", configuration["IdentityServer:Scope"] ?? throw new InvalidOperationException("Scope is not configured.") }
            })
        };

        var client = httpClientFactory.CreateClient("IdentityServerClient");
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = System.Text.Json.JsonSerializer.Deserialize<AuthServerResponse>(responseContent, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (loginResponse is null)
            {
                logger.LogError("Deserialization of login response failed. {content}", responseContent);

                throw new InvalidOperationException("Failed to deserialize login response.");
            }

            return new UserLoginResponseDTO(true, loginResponse.AccessToken, string.Empty);
        }
        else
        {
            logger.LogError("Login failed for user {Username}. Status code: {StatusCode}, Reason: {Reason}",
                loginRequest.Username, response.StatusCode, response.ReasonPhrase);

            return new UserLoginResponseDTO(false, "Login failed: ", response.ReasonPhrase);
        }
    }
}
