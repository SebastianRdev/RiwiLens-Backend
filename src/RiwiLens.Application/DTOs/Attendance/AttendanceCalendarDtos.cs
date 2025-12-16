namespace src.RiwiLens.Application.DTOs.Attendance;

public class AttendanceCalendarResponseDto
{
    public bool Success { get; set; } = true;
    public AttendanceCalendarDataDto Data { get; set; } = new();
}

public class AttendanceCalendarDataDto
{
    public string CoderId { get; set; } = string.Empty;
    public string CoderName { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public List<AttendanceRecordDto> Records { get; set; } = new();
    public AttendanceSummaryDto Summary { get; set; } = new();
}

public class AttendanceRecordDto
{
    public string Date { get; set; } = string.Empty; // YYYY-MM-DD
    public string Status { get; set; } = string.Empty; // present, absent, justified
}

public class AttendanceSummaryDto
{
    public int TotalPresent { get; set; }
    public int TotalAbsent { get; set; }
    public int TotalJustified { get; set; }
}
