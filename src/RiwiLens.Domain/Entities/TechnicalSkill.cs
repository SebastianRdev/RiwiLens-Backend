namespace src.RiwiLens.Domain.Entities;

// Catalog of technical skills of the system.
public class TechnicalSkill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public CategoryTechnicalSkill Category { get; set; }
    public ICollection<CoderTechnicalSkill> CoderTechnicalSkills { get; set; }
}