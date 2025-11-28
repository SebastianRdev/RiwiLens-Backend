namespace RiwiLens.Domain.Entities;

// Define the blocks or shifts of the day (e.g., Morning, Afternoon), with their start and end times.
public class Day
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public ICollection<Class> Classes { get; set; }
}