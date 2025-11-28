using RiwiLens.Domain.Enums;

namespace RiwiLens.Domain.Entities;

// Relationship M: M that connects a TeamLeader with the specialties he masters, with level.
public class TeamLeaderSpecialty
{
    public int Id { get; set; }
    public string TeamLeaderId { get; set; }
    public int SpecialtyId { get; set; }
    public TeamLeaderSpecialtyLevel Level { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public TeamLeader TeamLeader { get; set; }
    public Specialty Specialty { get; set; }
}