using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using src.RiwiLens.Application.Interfaces;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Application.Services;
using src.RiwiLens.Infrastructure.Repositories;
using src.RiwiLens.Infrastructure.Services;
using src.RiwiLens.Infrastructure.Services.Identity;
using src.RiwiLens.Infrastructure.Services.IA;
using Google.GenAI;

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

        // Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(Repositories.GenericRepository<>));

        // Services
        services.AddScoped<ICoderService, CoderService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IClanService, ClanService>();
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ICvService, CvService>();
        
        services.AddSingleton<Client>(sp => 
        {
            var apiKey = configuration["GeminiApiKey"];
            return new Client(apiKey: apiKey);
        });
        services.AddScoped<IAiService, GeminiService>();

        return services;
    }
}
