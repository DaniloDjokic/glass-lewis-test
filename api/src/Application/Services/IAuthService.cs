using Application.DTOs;

namespace Application.Services;

public interface IAuthService
{
    Task<UserLoginResponseDTO> LoginAsync(UserLoginRequestDTO loginRequest);
}
