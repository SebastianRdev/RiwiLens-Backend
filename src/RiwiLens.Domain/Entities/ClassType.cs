namespace src.RiwiLens.Domain.Entities;

// Catalog of existing class types (English, Development, etc.).
public class ClassType
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public ICollection<Class> Classes { get; private set; } = new List<Class>();

    protected ClassType() { } // EF Core

    private ClassType(string name)
    {
        Name = name;
    }

    public static ClassType Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Class type name is required.");

        return new ClassType(name.Trim());
    }

    public void Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Class type name is required.");

        Name = name.Trim();
    }
}
