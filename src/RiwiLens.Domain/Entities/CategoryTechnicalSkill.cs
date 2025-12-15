namespace src.RiwiLens.Domain.Entities;

// Classify the technical skills into categories.
public class CategoryTechnicalSkill
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public ICollection<TechnicalSkill> TechnicalSkills { get; private set; } = new List<TechnicalSkill>();

    protected CategoryTechnicalSkill() { } // EF Core

    private CategoryTechnicalSkill(string name)
    {
        Name = name;
    }

    public static CategoryTechnicalSkill Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.");

        return new CategoryTechnicalSkill(name.Trim());
    }

    public void Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.");

        Name = name.Trim();
    }
}
