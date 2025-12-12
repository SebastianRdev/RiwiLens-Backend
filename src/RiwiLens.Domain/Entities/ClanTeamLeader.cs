namespace src.RiwiLens.Domain.Entities;

// M:N relationship between clan and TL, indicating what role the TL fulfills and on what dates it was assigned.
public class ClanTeamLeader
{
    public int Id { get; set; }
    public int ClanId { get; set; }
    public string TeamLeaderId { get; set; }
    public int RoleTeamLeaderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Clan Clan { get; set; }
    public TeamLeader TeamLeader { get; set; }
    public RoleTeamLeader RoleTeamLeader { get; set; }
}