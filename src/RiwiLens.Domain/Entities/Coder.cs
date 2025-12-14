using src.RiwiLens.Domain.Enums;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Domain.Entities;

// Main information of the coder (personal data, document, location, status, etc.).
public class Coder
{
    public int Id { get; private set; }

    public string FullName { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty; // FK ApplicationUser

    public DocumentType DocumentType { get; private set; }
    public string Identification { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;
    public DateTime BirthDate { get; private set; }

    public int ProfessionalProfileId { get; private set; }

    public string Country { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;

    public Gender Gender { get; private set; }

    public int StatusId { get; private set; }
    public StatusCoder? Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ProfessionalProfile ProfessionalProfile { get; private set; } = default!;
    public ICollection<CoderSoftSkill> SoftSkills { get; private set; } = new List<CoderSoftSkill>();
    public ICollection<CoderTechnicalSkill> TechnicalSkills { get; private set; } = new List<CoderTechnicalSkill>();
    public ICollection<Feedback> Feedback { get; private set; } = new List<Feedback>();
    public ICollection<ClanCoder> ClanCoders { get; private set; } = new List<ClanCoder>();
    public ICollection<Attendance> Attendances { get; private set; } = new List<Attendance>();
    public ICollection<FaceCollection> FaceCollections { get; private set; } = new List<FaceCollection>();
    public ICollection<Notification> Notifications { get; private set; } = new List<Notification>();

    // EF
    private Coder() { }

    public static Coder Create(
        string fullName,
        string userId,
        DocumentType documentType,
        string identification,
        DateTime birthDate,
        string address,
        string country,
        string city,
        Gender gender,
        int professionalProfileId,
        int statusId)
    {
        ValidateEnums(documentType, gender);

        return new Coder
        {
            FullName = fullName,
            UserId = userId,
            DocumentType = documentType,
            Identification = identification,
            BirthDate = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc),
            Address = address,
            Country = country,
            City = city,
            Gender = gender,
            ProfessionalProfileId = professionalProfileId,
            StatusId = statusId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    // UPDATES

    public void UpdatePersonalInfo(
        string fullName,
        DateTime birthDate,
        Gender gender)
    {
        if (gender == Gender.Unknown)
            throw new InvalidOperationException("Invalid gender.");

        FullName = fullName;
        BirthDate = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc);
        Gender = gender;
        Touch();
    }

    public void UpdateLocation(string address, string country, string city)
    {
        Address = address;
        Country = country;
        City = city;
        Touch();
    }

    public void ChangeDocument(DocumentType documentType, string identification)
    {
        if (documentType == DocumentType.Unknown)
            throw new InvalidOperationException("Invalid document type.");

        DocumentType = documentType;
        Identification = identification;
        Touch();
    }

    public void ChangeStatus(int statusId)
    {
        StatusId = statusId;
        Touch();
    }

    // Helpers
    private static void ValidateEnums(DocumentType documentType, Gender gender)
    {
        if (documentType == DocumentType.Unknown)
            throw new InvalidOperationException("Invalid document type.");

        if (gender == Gender.Unknown)
            throw new InvalidOperationException("Invalid gender.");
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
