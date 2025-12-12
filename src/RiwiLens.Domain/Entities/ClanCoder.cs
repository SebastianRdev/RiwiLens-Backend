namespace src.RiwiLens.Domain.Entities;

// M:M relationship that records which coder belongs to which clan, with dates and status.
public class ClanCoder
{
    public int Id { get; set; }
    public int ClanId { get; set; }
    public string CoderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Clan Clan { get; set; }
    public Coder Coder { get; set; }
}