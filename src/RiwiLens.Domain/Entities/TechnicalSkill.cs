namespace src.RiwiLens.Domain.Entities;

// Catalog of technical skills of the system.
public class TechnicalSkill
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int CategoryId { get; private set; }

    public CategoryTechnicalSkill Category { get; private set; } = default!;
    public ICollection<CoderTechnicalSkill> CoderTechnicalSkills { get; private set; } = new List<CoderTechnicalSkill>();

    protected TechnicalSkill() { }

    public TechnicalSkill(string name, int categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");
        if (categoryId <= 0)
            throw new ArgumentException("Invalida CategoryId.");

        Name = name;
        CategoryId = categoryId;
    }
}
