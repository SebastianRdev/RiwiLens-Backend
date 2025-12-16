using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Attendance;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,TeamLeader")] // Or Coder if self-registration is allowed (usually automated)
    public async Task<ActionResult<AttendanceResponseDto>> Register([FromBody] RegisterAttendanceDto dto)
    {
        try
        {
            var attendance = await _attendanceService.RegisterAsync(dto);
            return CreatedAtAction(nameof(GetByClass), new { classId = dto.ClassId }, attendance);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,TeamLeader")]
    public async Task<ActionResult<AttendanceResponseDto>> Update(int id, [FromBody] UpdateAttendanceDto dto)
    {
        try
        {
            var updated = await _attendanceService.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("class/{classId}")]
    [Authorize(Roles = "Admin,TeamLeader")]
    public async Task<ActionResult<IEnumerable<AttendanceResponseDto>>> GetByClass(int classId)
    {
        return Ok(await _attendanceService.GetByClassIdAsync(classId));
    }

    [HttpGet("coder/{coderId}")]
    public async Task<ActionResult<IEnumerable<AttendanceResponseDto>>> GetByCoder(int coderId)
    {
        // TODO: Validate if current user is the coder or Admin/TL
        return Ok(await _attendanceService.GetByCoderIdAsync(coderId));
    }

    [HttpGet("clan/{clanId}/date/{date}")]
    [Authorize(Roles = "Admin,TeamLeader")]
    public async Task<ActionResult<IEnumerable<AttendanceResponseDto>>> GetByClanAndDate(int clanId, DateTime date)
    {
        return Ok(await _attendanceService.GetByClanIdAndDateAsync(clanId, date));
    }
}
