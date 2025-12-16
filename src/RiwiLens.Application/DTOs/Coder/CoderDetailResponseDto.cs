using src.RiwiLens.Application.DTOs.Coder;

namespace src.RiwiLens.Application.DTOs.Coder;

public class CoderDetailResponseDto : CoderResponseDto
{
    public string AboutMe { get; set; } = string.Empty;
    public string LinkedIn { get; set; } = string.Empty;
    public string GitHub { get; set; } = string.Empty;
    public string Portfolio { get; set; } = string.Empty;
    public List<string> TechnicalSkills { get; set; } = new();
    public List<string> SoftSkills { get; set; } = new();
}
