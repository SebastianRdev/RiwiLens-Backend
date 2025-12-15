using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.DTOs.Feedback;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,TeamLeader")]
    public async Task<ActionResult<FeedbackResponseDto>> Create([FromBody] CreateFeedbackDto dto)
    {
        try
        {
            var feedback = await _feedbackService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByCoder), new { coderId = feedback.CoderId }, feedback);
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

    [HttpGet("coder/{coderId}")]
    public async Task<ActionResult<IEnumerable<FeedbackResponseDto>>> GetByCoder(int coderId)
    {
        // TODO: Validate user access
        return Ok(await _feedbackService.GetByCoderIdAsync(coderId));
    }

    [HttpGet("teamleader/{teamLeaderId}")]
    [Authorize(Roles = "Admin,TeamLeader")]
    public async Task<ActionResult<IEnumerable<FeedbackResponseDto>>> GetByTeamLeader(int teamLeaderId)
    {
        return Ok(await _feedbackService.GetByTeamLeaderIdAsync(teamLeaderId));
    }
}
