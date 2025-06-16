using Application.Common;
using Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthService(IHttpClientFactory httpClientFactory, ILogger<AuthService> logger) : IAuthService
{
    public async Task<UserLoginResponseDTO> LoginAsync(UserLoginRequestDTO loginRequest)
    {
        logger.LogInformation("Attempting to log in user: {Username}", loginRequest.Username);

        // TODO: Move to config
        var identityServerUrl = "http://localhost:5272/connect/token";

        var request = new HttpRequestMessage(HttpMethod.Post, identityServerUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
			// TODO: Move to config
                { "grant_type", "password" },
                { "username", loginRequest.Username },
                { "password", loginRequest.Password },
                { "client_id", "glass-lewis-api-client" },
                { "client_secret", "very-secure-development-secret" },
                { "scope", "api" }
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
