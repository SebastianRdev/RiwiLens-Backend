namespace RiwiLens.Domain.Entities;

// M:M relationship between coder and their technical skills, with mastery level.
public class CoderTechnicalSkill
{
    public int Id { get; set; }
    public int CoderId { get; set; }
    public int SkillId { get; set; }
    public string Level { get; set; } = string.Empty;
}