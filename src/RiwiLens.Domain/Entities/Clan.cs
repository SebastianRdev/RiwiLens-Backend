namespace src.RiwiLens.Domain.Entities;

// Coders group. It has a name, description, and expiration dates.
public class Clan
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<ClanCoder> ClanCoders { get; private set; } = new List<ClanCoder>();
    public ICollection<ClanTeamLeader> ClanTeamLeaders { get; private set; } = new List<ClanTeamLeader>();

    private Clan() { }

    private Clan(string name, string description)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Clan Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Clan name is required.");

        return new Clan(name, description);
    }

    public void UpdateInfo(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
