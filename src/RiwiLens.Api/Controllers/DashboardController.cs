using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Dashboard;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetGlobalStats()
    {
        return Ok(await _dashboardService.GetGlobalStatsAsync());
    }
}
