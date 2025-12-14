using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Domain.Entities;

// Catalog of specialties for the TeamLeader (e.g., English, development, public speaking).
public class Specialty
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public ICollection<TeamLeaderSpecialty> TeamLeaderSpecialties { get; private set; } = new List<TeamLeaderSpecialty>();

    protected Specialty() { }

    public Specialty(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        Name = name;
        Description = description;
    }
}
