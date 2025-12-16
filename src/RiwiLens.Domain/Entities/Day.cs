namespace src.RiwiLens.Domain.Entities;

// Define the blocks or shifts of the day (e.g., Morning, Afternoon), with their start and end times.
public class Day
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public ICollection<Class> Classes { get; private set; } = new List<Class>();

    protected Day() { } // EF Core

    private Day(string name, TimeSpan start, TimeSpan end)
    {
        Name = name;
        StartTime = start;
        EndTime = end;
    }

    public static Day Create(string name, TimeSpan start, TimeSpan end)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Day name is required.");

        if (end <= start)
            throw new ArgumentException("Invalid schedule.");

        return new Day(name, start, end);
    }

    public void Update(string name, TimeSpan start, TimeSpan end)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Day name is required.");

        if (end <= start)
            throw new ArgumentException("Invalid schedule.");

        Name = name;
        StartTime = start;
        EndTime = end;
    }
}
