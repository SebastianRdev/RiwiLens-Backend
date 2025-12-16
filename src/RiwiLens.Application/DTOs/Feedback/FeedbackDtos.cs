namespace src.RiwiLens.Application.DTOs.Feedback;

public class FeedbackResponseDto
{
    public int Id { get; set; }
    public int CoderId { get; set; }
    public string CoderName { get; set; } = string.Empty;
    public int TeamLeaderId { get; set; }
    public string TeamLeaderName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateFeedbackDto
{
    public int CoderId { get; set; }
    public int TeamLeaderId { get; set; }
    public string Message { get; set; } = string.Empty;
}
