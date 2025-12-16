namespace src.RiwiLens.Domain.Entities;

// It represents a specific class taught to a clan on a particular day and session, by a TL. It includes the date, time, and type of class.
public class Class
{
    public int Id { get; private set; }
    public DateTime Date { get; private set; }
    public int DayId { get; private set; }
    public int ClassTypeId { get; private set; }
    public int TeamLeaderId { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Day Day { get; private set; } = default!;
    public ClassType ClassType { get; private set; } = default!;
    public TeamLeader TeamLeader { get; private set; } = default!;

    private Class() { }

    private Class(
        DateTime date,
        int dayId,
        int classTypeId,
        int teamLeaderId,
        TimeSpan start,
        TimeSpan end)
    {
        Date = date;
        DayId = dayId;
        ClassTypeId = classTypeId;
        TeamLeaderId = teamLeaderId;
        StartTime = start;
        EndTime = end;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Class Create(
        DateTime date,
        int dayId,
        int classTypeId,
        int teamLeaderId,
        TimeSpan start,
        TimeSpan end)
    {
        if (end <= start)
            throw new ArgumentException("The end time must be later.");

        return new Class(date, dayId, classTypeId, teamLeaderId, start, end);
    }

    public void Update(
        DateTime date,
        int dayId,
        int classTypeId,
        int teamLeaderId,
        TimeSpan start,
        TimeSpan end)
    {
        if (end <= start)
            throw new ArgumentException("The end time must be later.");

        Date = date;
        DayId = dayId;
        ClassTypeId = classTypeId;
        TeamLeaderId = teamLeaderId;
        StartTime = start;
        EndTime = end;
        UpdatedAt = DateTime.UtcNow;
    }
}
