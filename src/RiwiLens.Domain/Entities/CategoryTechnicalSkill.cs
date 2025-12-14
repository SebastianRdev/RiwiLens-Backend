namespace src.RiwiLens.Domain.Entities;

// Classify the technical skills into categories.
public class CategoryTechnicalSkill
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public ICollection<TechnicalSkill> TechnicalSkills { get; private set; } = new List<TechnicalSkill>();

    protected CategoryTechnicalSkill() { }

    public CategoryTechnicalSkill(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        Name = name;
    }
}
