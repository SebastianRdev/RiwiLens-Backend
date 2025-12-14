namespace src.RiwiLens.Domain.Entities;

public class RoleTeamLeader
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public ICollection<ClanTeamLeader> ClanTeamLeaders { get; private set; } = new List<ClanTeamLeader>();

    protected RoleTeamLeader() { }

    public RoleTeamLeader(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required.");

        Name = name;
        Description = description;
    }
}

