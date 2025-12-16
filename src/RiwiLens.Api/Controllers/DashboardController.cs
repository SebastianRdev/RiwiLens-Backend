using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Dashboard;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DashboardStatsDto>> GetGlobalStats()
    {
        return Ok(await _dashboardService.GetGlobalStatsAsync());
    }

    [HttpGet("user-management-stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserManagementStatsDto>> GetUserManagementStats()
    {
        return Ok(await _dashboardService.GetUserManagementStatsAsync());
    }

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
    {
        return Ok(await _dashboardService.GetUsersAsync());
    }

    [HttpGet("coder/{coderId}")]
    [Authorize(Roles = "Admin,TeamLeader,Coder")]
    public async Task<ActionResult<CoderDashboardDto>> GetCoderDashboard(int coderId)
    {
        try
        {
            return Ok(await _dashboardService.GetCoderDashboardAsync(coderId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
