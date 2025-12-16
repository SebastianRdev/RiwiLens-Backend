using System.Security.Claims;

namespace src.RiwiLens.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(
        string userId,
        string email,
        string userName,
        IEnumerable<string> roles,
        int? numericId = null,
        IEnumerable<Claim>? extraClaims = null
    );
}
