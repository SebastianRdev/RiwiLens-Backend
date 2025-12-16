using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using src.RiwiLens.Application.Interfaces;
using src.RiwiLens.Infrastructure.Services.Identity;

namespace src.RiwiLens.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Auth
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
