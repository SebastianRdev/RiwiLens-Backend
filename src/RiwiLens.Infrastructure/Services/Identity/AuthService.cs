using Microsoft.AspNetCore.Identity;
using src.RiwiLens.Application.Interfaces;
using src.RiwiLens.Application.Common.Auth;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Services.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<TeamLeader> _tlRepository;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<TeamLeader> tlRepository)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _coderRepository = coderRepository;
        _tlRepository = tlRepository;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return AuthResult.Fail("Credenciales inválidas.");

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            return AuthResult.Fail("Credenciales inválidas.");

        var roles = await _userManager.GetRolesAsync(user);

        int? numericId = null;
        if (roles.Contains("Coder"))
        {
            var coders = await _coderRepository.FindAsync(c => c.UserId == user.Id);
            var coder = coders.FirstOrDefault();
            if (coder != null) numericId = coder.Id;
        }
        else if (roles.Contains("TeamLeader"))
        {
            var tls = await _tlRepository.FindAsync(t => t.UserId == user.Id);
            var tl = tls.FirstOrDefault();
            if (tl != null) numericId = tl.Id;
        }

        var token = _jwtService.GenerateToken(
            userId: user.Id,
            email: user.Email ?? string.Empty,
            userName: user.UserName ?? string.Empty,
            roles: roles,
            numericId: numericId
        );

        return AuthResult.Ok(token);
    }

    public Task<AuthResult> LogoutAsync()
    {
        // JWT es stateless → no hay nada que invalidar en servidor
        return Task.FromResult(AuthResult.Ok(""));
    }
}
