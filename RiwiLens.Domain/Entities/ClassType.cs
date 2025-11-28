namespace RiwiLens.Domain.Entities;

// Catalog of existing class types (English, Development, etc.).
public class ClassType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Class> Classes { get; set; }
}