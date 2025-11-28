namespace RiwiLens.Domain.Entities;

// M:M relationship between coder and their soft skills, with assigned level.
public class CoderSoftSkill
{
    public int Id { get; set; }
    public int CoderId { get; set; }
    public int SoftSkillId { get; set; }
}