namespace src.RiwiLens.Domain.Entities;

// Professional data of the coder (portfolio, links, about_me, profile percentage).
public class ProfessionalProfile
{
    public int Id { get; private set; }
    public string AboutMe { get; private set; } = string.Empty;
    public string LinkedIn { get; private set; } = string.Empty;
    public string GitHub { get; private set; } = string.Empty;
    public string Portfolio { get; private set; } = string.Empty;
    public decimal PercentageProfile { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<Coder> Coders { get; private set; } = new List<Coder>();

    private ProfessionalProfile() { }

    private ProfessionalProfile(string aboutMe)
    {
        AboutMe = aboutMe;
        PercentageProfile = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static ProfessionalProfile Create(string aboutMe)
    {
        return new ProfessionalProfile(aboutMe);
    }

    public void UpdateLinks(string linkedIn, string gitHub, string portfolio)
    {
        LinkedIn = linkedIn;
        GitHub = gitHub;
        Portfolio = portfolio;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfileCompletion(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Invalid porcentage");

        PercentageProfile = percentage;
        UpdatedAt = DateTime.UtcNow;
    }
}
