using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.Interfaces;
using src.RiwiLens.Application.DTOs.Auth;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Domain.Entities;
namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<TeamLeader> _tlRepository;

    public AuthController(
        IAuthService authService,
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<TeamLeader> tlRepository)
    {
        _authService = authService;
        _coderRepository = coderRepository;
        _tlRepository = tlRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto.Email, dto.Password);

        if (!result.Success)
            return Unauthorized(result.Message);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps, // Dynamic: true if HTTPS, false if HTTP
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTime.UtcNow.AddMinutes(60)
        };

        Response.Cookies.Append("access_token", result.Token, cookieOptions);

        return Ok(new { message = "Login successful" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Path = "/"
        });

        return Ok(new { message = "Logout successful" });
    }

    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> Me()
    {
        var user = HttpContext.User;
        var id = user.FindFirst("id")?.Value ?? user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var uuid = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = user.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var role = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(uuid)) return Unauthorized();

        object response = new { id, uuid, email, name, role };

        if (role == "Coder")
        {
            var coders = await _coderRepository.FindAsync(c => c.UserId == uuid);
            var coder = coders.FirstOrDefault();
            if (coder != null)
            {
                response = new
                {
                    id = coder.Id, // Numeric ID
                    uuid,
                    email,
                    name = coder.FullName,
                    role,
                    documentType = coder.DocumentType.ToString(),
                    documentNumber = coder.Identification,
                    gender = coder.Gender.ToString(),
                    isActive = coder.StatusId == 1, // Assuming 1 is Active
                    profileImageUrl = "" // Placeholder
                };
            }
        }
        else if (role == "TeamLeader")
        {
            var tls = await _tlRepository.FindAsync(t => t.UserId == uuid);
            var tl = tls.FirstOrDefault();
            if (tl != null)
            {
                response = new
                {
                    id = tl.Id, // Numeric ID
                    uuid,
                    email,
                    name = tl.FullName,
                    role,
                    documentType = "", // TL doesn't have doc type in entity
                    documentNumber = "",
                    gender = tl.Gender.ToString(),
                    isActive = true, // TL doesn't have status
                    profileImageUrl = ""
                };
            }
        }

        return Ok(response);
    }
}
