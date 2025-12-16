using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Domain.Entities;

// Verified attendance record of a coder in a class
public class Attendance
{
    public int Id { get; private set; }

    public int ClanId { get; private set; }
    public int ClassId { get; private set; }
    public int CoderId { get; private set; }

    public DateTime TimestampIn { get; private set; }

    public AttendanceStatus Status { get; private set; }
    public string VerifiedBy { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Clan Clan { get; private set; } = default!;
    public Class Class { get; private set; } = default!;
    public Coder Coder { get; private set; } = default!;

    private Attendance() { } // EF Core

    private Attendance(
        int clanId,
        int classId,
        int coderId,
        DateTime timestampIn,
        string verifiedBy,
        string imageUrl)
    {
        ClanId = clanId;
        ClassId = classId;
        CoderId = coderId;
        TimestampIn = timestampIn;

        Status = AttendanceStatus.Present;
        VerifiedBy = verifiedBy;
        ImageUrl = imageUrl;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Attendance Create(
        int clanId,
        int classId,
        int coderId,
        DateTime timestampIn,
        string verifiedBy,
        string imageUrl)
    {
        if (clanId <= 0) throw new ArgumentException("Invalid clan.");
        if (classId <= 0) throw new ArgumentException("Invalid class.");
        if (coderId <= 0) throw new ArgumentException("Invalid coder.");
        if (timestampIn == default) throw new ArgumentException("Invalid timestamp.");

        return new Attendance(clanId, classId, coderId, timestampIn, verifiedBy, imageUrl);
    }

    public void MarkAsJustified(string justification)
    {
        if (string.IsNullOrWhiteSpace(justification))
            throw new ArgumentException("Justification is mandatory.");

        Status = AttendanceStatus.Justified;
        VerifiedBy = "MANUAL";
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsAbsent()
    {
        Status = AttendanceStatus.Absent;
        VerifiedBy = "SYSTEM";
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPresent(string verifiedBy)
    {
        if (string.IsNullOrWhiteSpace(verifiedBy))
            throw new ArgumentException("Verification method required.");

        Status = AttendanceStatus.Present;
        VerifiedBy = verifiedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(AttendanceStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
