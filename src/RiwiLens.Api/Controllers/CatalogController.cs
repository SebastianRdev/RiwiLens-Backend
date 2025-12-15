using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Catalog;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("technical-skills")]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetTechnicalSkills()
    {
        return Ok(await _catalogService.GetTechnicalSkillsAsync());
    }

    [HttpGet("soft-skills")]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetSoftSkills()
    {
        return Ok(await _catalogService.GetSoftSkillsAsync());
    }

    [HttpGet("class-types")]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetClassTypes()
    {
        return Ok(await _catalogService.GetClassTypesAsync());
    }

    [HttpGet("days")]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetDays()
    {
        return Ok(await _catalogService.GetDaysAsync());
    }



    [HttpGet("specialties")]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetSpecialties()
    {
        return Ok(await _catalogService.GetSpecialtiesAsync());
    }

    [HttpGet("status-coders")]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetStatusCoders()
    {
        return Ok(await _catalogService.GetStatusCodersAsync());
    }
}
