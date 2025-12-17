using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.Interfaces;
using src.RiwiLens.Application.DTOs.Auth;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Domain.Entities;
using System.Linq;
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
        IGenericRepository<TeamLeader> tlRepository,
        IGenericRepository<ClanCoder> clanCoderRepository,
        IGenericRepository<ClanTeamLeader> clanTlRepository,
        IGenericRepository<Clan> clanRepository)
    {
        _authService = authService;
        _coderRepository = coderRepository;
        _tlRepository = tlRepository;
        _clanCoderRepository = clanCoderRepository;
        _clanTlRepository = clanTlRepository;
        _clanRepository = clanRepository;
    }

    private readonly IGenericRepository<ClanCoder> _clanCoderRepository;
    private readonly IGenericRepository<ClanTeamLeader> _clanTlRepository;
    private readonly IGenericRepository<Clan> _clanRepository;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto.Email, dto.Password);

        if (!result.Success)
            return Unauthorized(result.Message);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = false, // Allow frontend to read cookie
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
            HttpOnly = false,
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
                var clanName = "N/A";
                var clanCoders = await _clanCoderRepository.FindAsync(cc => cc.CoderId == coder.Id && cc.IsActive);
                if (clanCoders.Any())
                {
                    var clan = await _clanRepository.GetByIdAsync(clanCoders.First().ClanId);
                    if (clan != null) clanName = clan.Name;
                }

                response = new
                {
                    id = coder.Id, // Numeric ID
                    uuid,
                    email,
                    name = coder.FullName,
                    role,
                    clan = clanName,
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
                var clanName = "N/A";
                var clanTls = await _clanTlRepository.FindAsync(ct => ct.TeamLeaderId == tl.Id);
                var activeClanTl = clanTls.FirstOrDefault(ct => ct.EndDate == null) 
                                   ?? clanTls.OrderByDescending(ct => ct.StartDate).FirstOrDefault();
                
                if (activeClanTl != null)
                {
                    var clan = await _clanRepository.GetByIdAsync(activeClanTl.ClanId);
                    if (clan != null) clanName = clan.Name;
                }

                response = new
                {
                    id = tl.Id, // Numeric ID
                    uuid,
                    email,
                    name = tl.FullName,
                    role,
                    clan = clanName,
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
