using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Domain.Entities;

// M:M relationship between coder and their technical skills, with mastery level.
public class CoderTechnicalSkill
{
    public int Id { get; set; }
    public string CoderId { get; set; }
    public int SkillId { get; set; }
    public TechnicalSkillLevel Level { get; set; }

    public Coder Coder { get; set; }
    public TechnicalSkill TechnicalSkill { get; set; }
}