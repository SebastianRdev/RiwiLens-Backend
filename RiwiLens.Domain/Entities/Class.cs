namespace RiwiLens.Domain.Entities;

// It represents a specific class taught to a clan on a particular day and session, by a TL. It includes the date, time, and type of class.
public class Class
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int DayId { get; set; }
    public int ClassTypeId { get; set; }
    public int TeamLeaderId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Day Day { get; set; }
    public ClassType ClassType { get; set; }
    public TeamLeader TeamLeader { get; set; }
}