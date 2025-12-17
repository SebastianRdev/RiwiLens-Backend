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

    [HttpGet("teamleader/{tlId}")]
    [Authorize(Roles = "Admin,TeamLeader")]
    public async Task<ActionResult<TeamLeaderDashboardDto>> GetTeamLeaderDashboard(int tlId)
    {
        try
        {
            return Ok(await _dashboardService.GetTeamLeaderDashboardAsync(tlId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("teamleader/coders")]
    [Authorize(Roles = "TeamLeader")]
    public async Task<IActionResult> GetCodersByClan()
    {
        try
        {
            // Get Numeric ID from Token
            var numericIdClaim = User.FindFirst("numericId")?.Value;
            if (string.IsNullOrEmpty(numericIdClaim) || !int.TryParse(numericIdClaim, out int tlId))
            {
                // Fallback: Get by UUID
                var uuid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(uuid)) return Unauthorized();
                
                 var idClaim = User.FindFirst("id")?.Value;
                 if (!string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out int id))
                 {
                     tlId = id;
                 }
                 else
                 {
                     return Unauthorized("User ID not found in token.");
                 }
            }

            var coders = await _dashboardService.GetCodersByClanAsync(tlId);
            return Ok(coders);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
