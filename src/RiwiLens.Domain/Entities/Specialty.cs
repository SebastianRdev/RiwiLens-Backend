namespace src.RiwiLens.Domain.Entities;

// Catalog of specialties for the TeamLeader (e.g., English, development, public speaking).
public class Specialty
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<TeamLeaderSpecialty> TeamLeaderSpecialties { get; set; }
}