using src.RiwiLens.Application.Common.Auth;

namespace src.RiwiLens.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> LogoutAsync();
}
