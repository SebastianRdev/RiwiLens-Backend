namespace src.RiwiLens.Domain.Entities;

// Catalog of soft skills (communication, leadership, etc.).
public class SoftSkill
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public ICollection<CoderSoftSkill> CoderSoftSkills { get; private set; } = new List<CoderSoftSkill>();

    protected SoftSkill() { }

    public SoftSkill(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        Name = name;
        Description = description;
    }
}
