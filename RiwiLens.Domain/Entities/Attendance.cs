namespace RiwiLens.Domain.Entities;

// Verified attendance record of a coder in a class: date/time, evidence, class and verification method (face, manual, etc.).
public class Attendance
{
    public int Id { get; set; }
    public int ClanId { get; set; }
    public int ClassId { get; set; }
    public int CoderId { get; set; }
    public DateTime TimestampIn { get; set; }
    public string VerifiedBy { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}