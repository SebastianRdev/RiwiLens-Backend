using Microsoft.AspNetCore.Identity;
using src.RiwiLens.Application.Interfaces;
using src.RiwiLens.Application.Common.Auth;
using src.RiwiLens.Infrastructure.Identity;

namespace src.RiwiLens.Infrastructure.Services.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return AuthResult.Failure("Credenciales inválidas.");

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            return AuthResult.Failure("Credenciales inválidas.");

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtService.GenerateToken(
            userId: user.Id,
            email: user.Email ?? string.Empty,
            userName: user.UserName ?? string.Empty,
            roles: roles
        );

        return AuthResult.SuccessResult(token);
    }

    public Task<AuthResult> LogoutAsync()
    {
        // JWT es stateless → no hay nada que invalidar en servidor
        return Task.FromResult(AuthResult.SuccessResult());
    }
}
