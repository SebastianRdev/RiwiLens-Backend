using src.RiwiLens.Application.DTOs.Dashboard;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetGlobalStatsAsync();
    Task<UserManagementStatsDto> GetUserManagementStatsAsync();
    Task<IEnumerable<UserResponseDto>> GetUsersAsync();
}
