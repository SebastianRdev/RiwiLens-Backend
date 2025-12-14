namespace src.RiwiLens.Domain.Entities;

// Define the blocks or shifts of the day (e.g., Morning, Afternoon), with their start and end times.
public class Day
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public ICollection<Class> Classes { get; private set; } = new List<Class>();

    protected Day() { }

    public Day(string name, TimeSpan start, TimeSpan end)
    {
        if (end <= start)
            throw new ArgumentException("Invalid schedule.");

        Name = name;
        StartTime = start;
        EndTime = end;
    }
}
