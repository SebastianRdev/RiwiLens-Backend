using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Coder;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CodersController : ControllerBase
{
    private readonly ICoderService _coderService;

    public CodersController(ICoderService coderService)
    {
        _coderService = coderService;
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
    public async Task<ActionResult<CoderResponseDto>> GetById(int id)
    {
        var coder = await _coderService.GetByIdAsync(id);
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
}
