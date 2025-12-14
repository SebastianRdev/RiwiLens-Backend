using src.RiwiLens.Domain.Enums;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Domain.Entities;

// Relationship M: M that between TeamLeader with the specialties he masters, with level.
public class TeamLeaderSpecialty
{
    public int Id { get; private set; }
    public int TeamLeaderId { get; private set; }
    public int SpecialtyId { get; private set; }

    public TeamLeaderSpecialtyLevel Level { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public TeamLeader TeamLeader { get; private set; } = default!;
    public Specialty Specialty { get; private set; } = default!;

    private TeamLeaderSpecialty() { }

    public static TeamLeaderSpecialty Create(
        int teamLeaderId,
        int specialtyId,
        TeamLeaderSpecialtyLevel level)
    {
        ValidateLevel(level);

        return new TeamLeaderSpecialty
        {
            TeamLeaderId = teamLeaderId,
            SpecialtyId = specialtyId,
            Level = level,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void ChangeLevel(TeamLeaderSpecialtyLevel newLevel)
    {
        ValidateLevel(newLevel);
        Level = newLevel;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateLevel(TeamLeaderSpecialtyLevel level)
    {
        if (level == TeamLeaderSpecialtyLevel.Unknown)
            throw new InvalidOperationException("Invalid specialty level.");
    }
}

