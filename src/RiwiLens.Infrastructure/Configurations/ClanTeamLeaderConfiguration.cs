using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class ClanTeamLeaderConfiguration : IEntityTypeConfiguration<ClanTeamLeader>
{
    public void Configure(EntityTypeBuilder<ClanTeamLeader> builder)
    {
        builder
            .HasOne(ct => ct.Clan)
            .WithMany(c => c.ClanTeamLeaders)
            .HasForeignKey(ct => ct.ClanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ct => ct.TeamLeader)
            .WithMany(tl => tl.ClanAssignments)
            .HasForeignKey(ct => ct.TeamLeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ct => ct.RoleTeamLeader)
            .WithMany()
            .HasForeignKey(ct => ct.RoleTeamLeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        // A TL cannot have the same role twice in the same clan
        builder
            .HasIndex(ct => new { ct.ClanId, ct.TeamLeaderId, ct.RoleTeamLeaderId })
            .IsUnique();
    }
}
