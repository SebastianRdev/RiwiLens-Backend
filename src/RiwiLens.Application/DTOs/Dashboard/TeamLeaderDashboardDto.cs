namespace src.RiwiLens.Application.DTOs.Dashboard;

public class TeamLeaderDashboardDto
{
    public int TotalCoders { get; set; }
    public double AverageAttendance { get; set; }
    public List<CoderSummaryDto> Coders { get; set; } = new();
}

public class CoderSummaryDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ProfileImageUrl { get; set; } = string.Empty;
}
