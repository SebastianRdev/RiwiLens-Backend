using src.RiwiLens.Domain.Enums;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Domain.Entities;

// TeamLeader domain entity
public class TeamLeader
{
    public int Id { get; private set; }
    public string UserId { get; private set; } = string.Empty; // FK ApplicationUser
    public string FullName { get; private set; } = string.Empty;

    public Gender Gender { get; private set; }
    public DateTime BirthDate { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<TeamLeaderSpecialty> TeamLeaderSpecialties { get; private set; } = new List<TeamLeaderSpecialty>();
    public ICollection<ClanTeamLeader> ClanAssignments { get; private set; } = new List<ClanTeamLeader>();
    public ICollection<Feedback> FeedbackGiven { get; private set; } = new List<Feedback>();
    public ICollection<Class> Classes { get; private set; } = new List<Class>();
    public ICollection<Notification> Notifications { get; private set; } = new List<Notification>();

    private TeamLeader() { } // EF Core

    public static TeamLeader Create(
        string userId,
        string fullName,
        Gender gender,
        DateTime birthDate)
    {
        ValidateEnums(gender);

        return new TeamLeader
        {
            UserId = userId,
            FullName = fullName,
            Gender = gender,
            BirthDate = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string fullName,
        Gender gender,
        DateTime birthDate)
    {
        ValidateEnums(gender);

        FullName = fullName;
        Gender = gender;
        BirthDate = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc);
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateEnums(Gender gender)
    {
        if (gender == Gender.Unknown)
            throw new InvalidOperationException("Invalid gender.");
    }
}
