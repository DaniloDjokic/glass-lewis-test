using System.Text.Json.Serialization;

namespace Application.Common;

public record AuthServerSuccessResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("expires_in")] int ExpiresIn,
    [property: JsonPropertyName("token_type")] string TokenType,
    [property: JsonPropertyName("scope")] string Scope
);

public record AuthServerErrorResponse(
    [property: JsonPropertyName("error")] string Error,
    [property: JsonPropertyName("error_description")] string ErrorDescription
);
