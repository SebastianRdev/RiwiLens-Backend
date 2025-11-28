namespace RiwiLens.Domain.Entities;

// Verified attendance record of a coder in a class: date/time, evidence, class and verification method (face, manual, etc.).
public class Attendance
{
    public int Id { get; set; }
    public int ClanId { get; set; }
    public Clan Clan { get; set; } = default!;
    public int ClassId { get; set; }
    public Class Class { get; set; } = default!;
    public int CoderId { get; set; }
    public Coder Coder { get; set; } = default!;
    public DateTime TimestampIn { get; set; }
    public string VerifiedBy { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}