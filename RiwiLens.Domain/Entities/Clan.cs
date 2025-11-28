namespace RiwiLens.Domain.Entities;

// Coders group. It has a name, description, and expiration dates.
public class Clan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}