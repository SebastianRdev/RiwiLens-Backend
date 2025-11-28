namespace RiwiLens.Domain.Entities;

public class StatusCoder
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Coder> Coders { get; set; }
}