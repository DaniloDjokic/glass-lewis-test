using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers;

[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    [Route("api/login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequestDTO loginRequest)
    {
        if (loginRequest is null)
        {
            return BadRequest("Login request cannot be null");
        }

        var response = await authService.LoginAsync(loginRequest);

        if (!response.Success)
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(new { Token = response.Token });
    }
}
