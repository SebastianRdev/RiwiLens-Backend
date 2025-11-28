namespace RiwiLens.Domain.Entities;

// TeamLeader basic data
public class TeamLeader : ApplicationUser
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<TeamLeaderSpecialty> TeamLeaderSpecialties { get; set; }
    public ICollection<Feedback> FeedbackGiven { get; set; }
    public ICollection<ClanTeamLeader> ClanAssignments { get; set; }
    public ICollection<Class> Classes { get; set; }
    public ICollection<Notification> Notifications { get; set; }
}