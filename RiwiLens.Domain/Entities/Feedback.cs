namespace RiwiLens.Domain.Entities;

// Messages or feedback that a TL leaves for a coder.
public class Feedback
{
    public int Id { get; set; }
    public int CoderId { get; set; }
    public int TeamLeaderId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}