using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class ClanConfiguration : IEntityTypeConfiguration<Clan>
{
    public void Configure(EntityTypeBuilder<Clan> builder)
    {
        builder
            .HasMany(c => c.ClanCoders)
            .WithOne(cc => cc.Clan)
            .HasForeignKey(cc => cc.ClanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.ClanTeamLeaders)
            .WithOne(ct => ct.Clan)
            .HasForeignKey(ct => ct.ClanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
