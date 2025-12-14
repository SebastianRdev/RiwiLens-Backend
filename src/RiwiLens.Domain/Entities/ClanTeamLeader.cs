namespace src.RiwiLens.Domain.Entities;

// M:N relationship between clan and TL, indicating what role the TL fulfills and on what dates it was assigned.
public class ClanTeamLeader
{
    public int Id { get; private set; }
    public int ClanId { get; private set; }
    public int TeamLeaderId { get; private set; }
    public int RoleTeamLeaderId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Clan Clan { get; private set; } = default!;
    public TeamLeader TeamLeader { get; private set; } = default!;
    public RoleTeamLeader RoleTeamLeader { get; private set; } = default!;

    private ClanTeamLeader() { }

    private ClanTeamLeader(int clanId, int tlId, int roleId)
    {
        ClanId = clanId;
        TeamLeaderId = tlId;
        RoleTeamLeaderId = roleId;
        StartDate = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static ClanTeamLeader Create(int clanId, int tlId, int roleId)
    {
        if (clanId <= 0) throw new ArgumentException("Invalid Clan.");
        if (tlId <= 0) throw new ArgumentException("Invalid TeamLeader.");
        if (roleId <= 0) throw new ArgumentException("Invalid Rol.");

        return new ClanTeamLeader(clanId, tlId, roleId);
    }

    public void EndAssignment()
    {
        EndDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
