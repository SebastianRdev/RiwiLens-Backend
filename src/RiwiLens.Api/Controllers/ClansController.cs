using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Clan;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ClansController : ControllerBase
{
    private readonly IClanService _clanService;

    public ClansController(IClanService clanService)
    {
        _clanService = clanService;
    }

    [HttpPost]
    public async Task<ActionResult<ClanResponseDto>> Create([FromBody] CreateClanDto dto)
    {
        try
        {
            var clan = await _clanService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = clan.Id }, clan);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClanResponseDto>>> GetAll()
    {
        return Ok(await _clanService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClanResponseDto>> GetById(int id)
    {
        var clan = await _clanService.GetByIdAsync(id);
        if (clan == null) return NotFound();
        return Ok(clan);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClanResponseDto>> Update(int id, [FromBody] UpdateClanDto dto)
    {
        try
        {
            var clan = await _clanService.UpdateAsync(id, dto);
            return Ok(clan);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _clanService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id}/assign-coder")]
    public async Task<ActionResult> AssignCoder(int id, [FromBody] AssignCoderDto dto)
    {
        try
        {
            await _clanService.AssignCoderAsync(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}/coder/{coderId}")]
    public async Task<ActionResult> RemoveCoder(int id, int coderId)
    {
        try
        {
            await _clanService.RemoveCoderAsync(id, coderId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id}/assign-team-leader")]
    public async Task<ActionResult> AssignTeamLeader(int id, [FromBody] AssignTeamLeaderDto dto)
    {
        try
        {
            await _clanService.AssignTeamLeaderAsync(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}/team-leader/{tlId}")]
    public async Task<ActionResult> RemoveTeamLeader(int id, int tlId)
    {
        try
        {
            await _clanService.RemoveTeamLeaderAsync(id, tlId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
