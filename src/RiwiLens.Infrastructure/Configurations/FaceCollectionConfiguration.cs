using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class FaceCollectionConfiguration : IEntityTypeConfiguration<FaceCollection>
{
    public void Configure(EntityTypeBuilder<FaceCollection> builder)
    {
        builder
            .HasOne(fc => fc.Coder)
            .WithMany(c => c.FaceCollections)
            .HasForeignKey(fc => fc.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(fc => fc.FaceId)
            .IsUnique();
    }
}
