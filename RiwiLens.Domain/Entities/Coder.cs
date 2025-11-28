using RiwiLens.Domain.Enums;

namespace RiwiLens.Domain.Entities;

// Main information of the coder (personal data, document, location, status, etc.).
public class Coder : ApplicationUser
{
    public DocumentType DocumentType { get; set; }
    public string Identification { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int ProfessionalProfileId { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int StatusId { get; set; }
    public virtual StatusCoder? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProfessionalProfile ProfessionalProfile { get; set; }
    public ICollection<CoderSoftSkill> SoftSkills { get; set; }
    public ICollection<CoderTechnicalSkill> TechnicalSkills { get; set; }
    public ICollection<Feedback> Feedback { get; set; }
    public ICollection<ClanCoder> ClanCoders { get; set; }
    public ICollection<Attendance> Attendances { get; set; }
    public ICollection<FaceCollection> FaceCollections { get; set; }
}