namespace src.RiwiLens.Domain.Entities;

// M:M relationship between coder and their soft skills, with assigned level.
public class CoderSoftSkill
{
    public int Id { get; private set; }
    public int CoderId { get; private set; }
    public int SoftSkillId { get; private set; }

    public Coder Coder { get; private set; } = default!;
    public SoftSkill SoftSkill { get; private set; } = default!;

    private CoderSoftSkill() { }

    private CoderSoftSkill(int coderId, int skillId)
    {
        CoderId = coderId;
        SoftSkillId = skillId;
    }

    public static CoderSoftSkill Create(int coderId, int skillId)
    {
        if (coderId <= 0) throw new ArgumentException("Invalid Coder.");
        if (skillId <= 0) throw new ArgumentException("Invalid Skill.");

        return new CoderSoftSkill(coderId, skillId);
    }
}
