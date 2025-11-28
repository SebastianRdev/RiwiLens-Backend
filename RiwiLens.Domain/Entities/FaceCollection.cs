namespace RiwiLens.Domain.Entities;

// AWS Rekognition related record: contains the collection_id, face_id, and URL of the base photo.
public class FaceCollection
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CollectionId { get; set; } = string.Empty;
    public string FaceId { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual ApplicationUser User { get; set; }
}