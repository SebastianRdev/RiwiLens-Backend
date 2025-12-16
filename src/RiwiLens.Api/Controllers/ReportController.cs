using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Report;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,TeamLeader")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("attendance")]
    public async Task<ActionResult<IEnumerable<AttendanceReportDto>>> GetAttendanceReport([FromQuery] int? clanId)
    {
        return Ok(await _reportService.GetAttendanceReportAsync(clanId));
    }

    [HttpGet("feedback")]
    public async Task<ActionResult<IEnumerable<FeedbackReportDto>>> GetFeedbackReport([FromQuery] int? clanId)
    {
        return Ok(await _reportService.GetFeedbackReportAsync(clanId));
    }
}
