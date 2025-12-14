using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Domain.Entities;

// M:M relationship between coder and their technical skills, with mastery level.
public class CoderTechnicalSkill
{
    public int Id { get; private set; }
    public int CoderId { get; private set; }
    public int SkillId { get; private set; }

    public TechnicalSkillLevel Level { get; private set; }

    public Coder Coder { get; private set; } = default!;
    public TechnicalSkill TechnicalSkill { get; private set; } = default!;

    private CoderTechnicalSkill() { } // EF

    public static CoderTechnicalSkill Create(
        int coderId,
        int skillId,
        TechnicalSkillLevel level)
    {
        ValidateLevel(level);

        return new CoderTechnicalSkill
        {
            CoderId = coderId,
            SkillId = skillId,
            Level = level
        };
    }

    public void ChangeLevel(TechnicalSkillLevel newLevel)
    {
        ValidateLevel(newLevel);
        Level = newLevel;
    }

    private static void ValidateLevel(TechnicalSkillLevel level)
    {
        if (level == TechnicalSkillLevel.Unknown)
            throw new InvalidOperationException("Invalid technical skill level.");
    }
}

