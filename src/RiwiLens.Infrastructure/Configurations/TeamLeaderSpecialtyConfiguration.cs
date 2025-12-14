using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class TeamLeaderSpecialtyConfiguration 
    : IEntityTypeConfiguration<TeamLeaderSpecialty>
{
    public void Configure(EntityTypeBuilder<TeamLeaderSpecialty> builder)
    {
        builder
            .HasOne(ts => ts.TeamLeader)
            .WithMany(tl => tl.TeamLeaderSpecialties)
            .HasForeignKey(ts => ts.TeamLeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ts => ts.Specialty)
            .WithMany(s => s.TeamLeaderSpecialties)
            .HasForeignKey(ts => ts.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        // A TL cannot repeat the same specialty
        builder
            .HasIndex(ts => new { ts.TeamLeaderId, ts.SpecialtyId })
            .IsUnique();
    }
}
