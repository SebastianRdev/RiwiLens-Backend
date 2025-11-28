namespace RiwiLens.Domain.Entities;

// Professional data of the coder (portfolio, links, about_me, profile percentage).
public class ProfessionalProfile
{
    public int Id { get; set; }
    public int CoderId { get; set; }
    public string AboutMe { get; set; } = string.Empty;
    public string LinkedIn { get; set; } = string.Empty;
    public string GitHub { get; set; } = string.Empty;
    public string Portfolio { get; set; } = string.Empty;
    public decimal PercentageProfile { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}