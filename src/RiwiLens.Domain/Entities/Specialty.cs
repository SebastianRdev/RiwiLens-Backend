using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Domain.Entities;

// Catalog of specialties for the TeamLeader (e.g., English, development, public speaking).
public class Specialty
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public ICollection<TeamLeaderSpecialty> TeamLeaderSpecialties { get; private set; }
        = new List<TeamLeaderSpecialty>();

    protected Specialty() { } // EF Core

    private Specialty(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Specialty Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Specialty name is required.");

        return new Specialty(name.Trim(), description);
    }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Specialty name is required.");

        Name = name.Trim();
        Description = description;
    }
}
