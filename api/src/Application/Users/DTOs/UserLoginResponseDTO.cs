namespace Application.DTOs;

public record UserLoginResponseDTO(bool Success, string? Token, string? ErrorMessage);
