namespace src.RiwiLens.Application.DTOs.Cv;

public class CvResponseDto
{
    public int CoderId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string CvContent { get; set; } = string.Empty; // Markdown content
    public DateTime GeneratedAt { get; set; }
}
