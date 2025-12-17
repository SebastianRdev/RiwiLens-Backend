using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.RiwiLens.Application.Interfaces.Services;

namespace src.RiwiLens.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PdfController : ControllerBase
{
    private readonly IPdfService _pdfService;

    public PdfController(IPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpGet("cv/{coderId}")]
    // [Authorize] // Uncomment to secure
    public async Task<IActionResult> GetCv(int coderId)
    {
        try
        {
            var pdfBytes = await _pdfService.GenerateCvAsync(coderId);
            return File(pdfBytes, "application/pdf", $"cv_coder_{coderId}.pdf");
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
