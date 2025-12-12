namespace src.RiwiLens.Domain.Entities;

// Classify the technical skills into categories.
public class CategoryTechnicalSkill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<TechnicalSkill> TechnicalSkills { get; set; }
}