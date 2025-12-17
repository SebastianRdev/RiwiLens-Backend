using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Attendance;
using src.RiwiLens.Application.DTOs.Coder;
using src.RiwiLens.Application.DTOs.User;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CodersController : ControllerBase
{
    private readonly ICoderService _coderService;
    private readonly IAttendanceService _attendanceService;
    private readonly IUserService _userService;

    public CodersController(ICoderService coderService, IAttendanceService attendanceService, IUserService userService)
    {
        _coderService = coderService;
        _attendanceService = attendanceService;
        _userService = userService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserDto dto)
    {
        dto.Role = "Coder";
        try 
        {
            var user = await _userService.CreateUserAsync(dto);
            // Return Created pointing to the user resource, as Coder ID is not immediately available in UserResponseDto (it has User ID)
            // We could fetch the Coder ID, but for now returning the User response is sufficient as per plan.
            return Created($"/api/users/{user.Id}", user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,TeamLeader")]
    public async Task<ActionResult<IEnumerable<CoderResponseDto>>> GetAll()
    {
        var coders = await _coderService.GetAllAsync();
        return Ok(coders);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,TeamLeader")]
    public async Task<ActionResult<CoderDetailResponseDto>> GetById(int id)
    {
        var coder = await _coderService.GetByIdAsync(id);
        if (coder == null) return NotFound();

        return Ok(coder);
    }

    [HttpGet("search/{identification}")]
    public async Task<ActionResult<CoderDetailResponseDto>> GetByIdentification(string identification)
    {
        var coder = await _coderService.GetByIdentificationAsync(identification);
        if (coder == null) return NotFound();

        return Ok(coder);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CoderResponseDto>> Update(int id, [FromBody] UpdateCoderDto dto)
    {
        // Optional: Check if the user is updating their own profile or is an Admin
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Logic to verify ownership can be added here if needed

        try
        {
            var updatedCoder = await _coderService.UpdateAsync(id, dto);
            return Ok(updatedCoder);
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


    [HttpGet("{coderId}/attendance")]
    public async Task<ActionResult<AttendanceCalendarResponseDto>> GetAttendanceCalendar(string coderId, [FromQuery] int? month, [FromQuery] int? year)
    {
        try
        {
            return Ok(await _attendanceService.GetAttendanceCalendarAsync(coderId, month, year));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
