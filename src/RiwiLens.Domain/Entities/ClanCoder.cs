namespace src.RiwiLens.Domain.Entities;

// M:M relationship that records which coder belongs to which clan, with dates and status.
public class ClanCoder
{
    public int Id { get; private set; }
    public int ClanId { get; private set; }
    public int CoderId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Clan Clan { get; private set; } = default!;
    public Coder Coder { get; private set; } = default!;

    private ClanCoder() { }

    private ClanCoder(int clanId, int coderId)
    {
        ClanId = clanId;
        CoderId = coderId;
        StartDate = DateTime.UtcNow;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static ClanCoder Create(int clanId, int coderId)
    {
        if (clanId <= 0) throw new ArgumentException("Invalid Clan.");
        if (coderId <= 0) throw new ArgumentException("Invalid Coder.");

        return new ClanCoder(clanId, coderId);
    }

    public void Deactivate()
    {
        IsActive = false;
        EndDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
