using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Application.DTOs.Attendance;

public class AttendanceResponseDto
{
    public int Id { get; set; }
    public int CoderId { get; set; }
    public string CoderName { get; set; } = string.Empty;
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? Remarks { get; set; } // Justification or notes
    public string? EvidenceUrl { get; set; } // S3 URL
}

public class RegisterAttendanceDto
{
    public int ClanId { get; set; } // Added ClanId
    public int CoderId { get; set; }
    public int ClassId { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }
    public string? EvidenceUrl { get; set; }
}

public class UpdateAttendanceDto
{
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }
}
