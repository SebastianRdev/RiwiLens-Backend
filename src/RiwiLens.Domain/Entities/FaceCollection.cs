namespace src.RiwiLens.Domain.Entities;

// AWS Rekognition related record: contains the collection_id, face_id, and URL of the base photo.
public class FaceCollection
{
    public int Id { get; private set; }
    public int CoderId { get; private set; }
    public string CollectionId { get; private set; } = string.Empty;
    public string FaceId { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Coder Coder { get; private set; } = default!;

    private FaceCollection() { }

    private FaceCollection(int coderId, string collectionId, string faceId, string imageUrl)
    {
        CoderId = coderId;
        CollectionId = collectionId;
        FaceId = faceId;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static FaceCollection Create(
        int coderId,
        string collectionId,
        string faceId,
        string imageUrl)
    {
        if (coderId <= 0) throw new ArgumentException("Invalid Coder.");

        return new FaceCollection(coderId, collectionId, faceId, imageUrl);
    }
}
