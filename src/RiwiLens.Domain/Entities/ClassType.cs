namespace src.RiwiLens.Domain.Entities;

// Catalog of existing class types (English, Development, etc.).
public class ClassType
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public ICollection<Class> Classes { get; private set; } = new List<Class>();

    protected ClassType() { }

    public ClassType(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        Name = name;
    }
}
