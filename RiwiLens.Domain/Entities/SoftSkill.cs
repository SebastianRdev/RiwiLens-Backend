namespace RiwiLens.Domain.Entities;

// Catalog of soft skills (communication, leadership, etc.).
public class SoftSkill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}