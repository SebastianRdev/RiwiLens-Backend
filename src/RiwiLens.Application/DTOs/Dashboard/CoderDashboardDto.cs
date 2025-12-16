namespace src.RiwiLens.Application.DTOs.Dashboard;

public class CoderDashboardDto
{
    public DashboardUserDto User { get; set; } = new();
    public DashboardAttendanceSummaryDto Attendance { get; set; } = new();
    public DashboardClanDto Clan { get; set; } = new();
    public List<RecentAttendanceDto> RecentAttendance { get; set; } = new();
    public LatestFeedbackDto? LatestFeedback { get; set; }
}

public class DashboardUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}

public class DashboardAttendanceSummaryDto
{
    public int Percentage { get; set; }
    public string Detail { get; set; } = string.Empty;
}

public class DashboardClanDto
{
    public string Name { get; set; } = string.Empty;
}

public class RecentAttendanceDto
{
    public string Date { get; set; } = string.Empty;
    public string Day { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class LatestFeedbackDto
{
    public FeedbackAuthorDto Author { get; set; } = new();
    public string Date { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class FeedbackAuthorDto
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
