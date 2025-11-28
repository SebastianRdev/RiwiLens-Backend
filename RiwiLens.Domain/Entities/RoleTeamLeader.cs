namespace RiwiLens.Domain.Entities;

// Catalog of roles that a TL can play within a clan (Development, English).
public class RoleTeamLeader
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}