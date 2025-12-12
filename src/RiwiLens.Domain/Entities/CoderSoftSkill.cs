namespace src.RiwiLens.Domain.Entities;

// M:M relationship between coder and their soft skills, with assigned level.
public class CoderSoftSkill
{
    public int Id { get; set; }
    public string CoderId { get; set; }
    public int SoftSkillId { get; set; }

    public Coder Coder { get; set; }
    public SoftSkill SoftSkill { get; set; }
}