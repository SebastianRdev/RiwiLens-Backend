using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class TeamLeaderConfiguration : IEntityTypeConfiguration<TeamLeader>
{
    public void Configure(EntityTypeBuilder<TeamLeader> builder)
    {
        builder.HasKey(n => n.Id);
        // TeamLeader → ApplicationUser (1-1)
        builder
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<TeamLeader>(tl => tl.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // TeamLeader → Specialties
        builder
            .HasMany(tl => tl.TeamLeaderSpecialties)
            .WithOne(ts => ts.TeamLeader)
            .HasForeignKey(ts => ts.TeamLeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        // TeamLeader → Clan assignments
        builder
            .HasMany(tl => tl.ClanAssignments)
            .WithOne(ct => ct.TeamLeader)
            .HasForeignKey(ct => ct.TeamLeaderId);

        // TeamLeader → Classes
        builder
            .HasMany(tl => tl.Classes)
            .WithOne(c => c.TeamLeader)
            .HasForeignKey(c => c.TeamLeaderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
