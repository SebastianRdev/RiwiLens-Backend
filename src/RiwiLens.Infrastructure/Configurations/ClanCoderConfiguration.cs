using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class ClanCoderConfiguration : IEntityTypeConfiguration<ClanCoder>
{
    public void Configure(EntityTypeBuilder<ClanCoder> builder)
    {
        builder
            .HasOne(cc => cc.Clan)
            .WithMany(c => c.ClanCoders)
            .HasForeignKey(cc => cc.ClanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(cc => cc.Coder)
            .WithMany(c => c.ClanCoders)
            .HasForeignKey(cc => cc.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        // A coder cannot be in the same clan twice.
        builder
            .HasIndex(cc => new { cc.ClanId, cc.CoderId })
            .IsUnique();
    }
}
