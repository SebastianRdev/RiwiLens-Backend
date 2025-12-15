using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Class;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ClassesController : ControllerBase
{
    private readonly IClassService _classService;

    public ClassesController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpPost]
    public async Task<ActionResult<ClassResponseDto>> Create([FromBody] CreateClassDto dto)
    {
        try
        {
            var newClass = await _classService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newClass.Id }, newClass);
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassResponseDto>>> GetAll()
    {
        return Ok(await _classService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClassResponseDto>> GetById(int id)
    {
        var c = await _classService.GetByIdAsync(id);
        if (c == null) return NotFound();
        return Ok(c);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClassResponseDto>> Update(int id, [FromBody] UpdateClassDto dto)
    {
        try
        {
            var updatedClass = await _classService.UpdateAsync(id, dto);
            return Ok(updatedClass);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _classService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("clan/{clanId}")]
    public async Task<ActionResult<IEnumerable<ClassResponseDto>>> GetByClan(int clanId)
    {
        return Ok(await _classService.GetByClanIdAsync(clanId));
    }

    [HttpGet("clan/{clanId}/today")]
    [Authorize(Roles = "Admin,TeamLeader")] // TLs need this for attendance
    public async Task<ActionResult<IEnumerable<ClassResponseDto>>> GetTodayByClan(int clanId)
    {
        return Ok(await _classService.GetTodayClassesByClanAsync(clanId));
    }
}
