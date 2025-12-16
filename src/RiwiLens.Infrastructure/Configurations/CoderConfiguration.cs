using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class CoderConfiguration : IEntityTypeConfiguration<Coder>
{
    public void Configure(EntityTypeBuilder<Coder> builder)
    {
        builder.HasKey(n => n.Id);
        // Coder → ApplicationUser (1-1)
        builder
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Coder>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Coder → ProfessionalProfile (M-1)
        builder
            .HasOne(c => c.ProfessionalProfile)
            .WithMany(p => p.Coders)
            .HasForeignKey(c => c.ProfessionalProfileId);

        // Coder → TechnicalSkills (1-M)
        builder
            .HasMany(c => c.TechnicalSkills)
            .WithOne(ts => ts.Coder)
            .HasForeignKey(ts => ts.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Coder → Attendances
        builder
            .HasMany(c => c.Attendances)
            .WithOne(a => a.Coder)
            .HasForeignKey(a => a.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Coder → FaceCollections
        builder
            .HasMany(c => c.FaceCollections)
            .WithOne(fc => fc.Coder)
            .HasForeignKey(fc => fc.CoderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
