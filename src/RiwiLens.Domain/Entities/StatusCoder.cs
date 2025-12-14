namespace src.RiwiLens.Domain.Entities;

public class StatusCoder
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public ICollection<Coder> Coders { get; private set; } = new List<Coder>();

    protected StatusCoder() { }

    public StatusCoder(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        Name = name;
    }
}
