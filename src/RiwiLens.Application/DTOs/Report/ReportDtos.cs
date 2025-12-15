namespace src.RiwiLens.Application.DTOs.Report;

public class AttendanceReportDto
{
    public int CoderId { get; set; }
    public string CoderName { get; set; } = string.Empty;
    public int TotalClasses { get; set; }
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public int JustifiedCount { get; set; }
    public double AttendancePercentage { get; set; }
}

public class FeedbackReportDto
{
    public int CoderId { get; set; }
    public string CoderName { get; set; } = string.Empty;
    public int TotalFeedbacks { get; set; }
    public DateTime? LastFeedbackDate { get; set; }
}
