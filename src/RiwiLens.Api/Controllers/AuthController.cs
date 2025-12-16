using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.Interfaces;
using src.RiwiLens.Application.DTOs.Auth;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto.Email, dto.Password);

        if (!result.Success)
            return Unauthorized(result.Message);

        return Ok(result);
    }
}
