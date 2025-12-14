namespace src.RiwiLens.Domain.Entities;

// Messages or feedback that a TL leaves for a coder.
public class Feedback
{
    public int Id { get; private set; }
    public int CoderId { get; private set; }
    public int TeamLeaderId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Coder Coder { get; private set; } = default!;
    public TeamLeader TeamLeader { get; private set; } = default!;

    private Feedback() { }

    private Feedback(int coderId, int teamLeaderId, string message)
    {
        CoderId = coderId;
        TeamLeaderId = teamLeaderId;
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }

    public static Feedback Create(int coderId, int teamLeaderId, string message)
    {
        if (coderId <= 0)
            throw new ArgumentException("Invalid Coder");
        if (teamLeaderId <= 0)
            throw new ArgumentException("Invalid TeamLeader");
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("The feedback cannot be empty.");

        return new Feedback(coderId, teamLeaderId, message);
    }
}
