using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class CoderTechnicalSkillConfiguration 
    : IEntityTypeConfiguration<CoderTechnicalSkill>
{
    public void Configure(EntityTypeBuilder<CoderTechnicalSkill> builder)
    {
        builder
            .HasOne(cts => cts.Coder)
            .WithMany(c => c.TechnicalSkills)
            .HasForeignKey(cts => cts.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(cts => cts.TechnicalSkill)
            .WithMany(ts => ts.CoderTechnicalSkills)
            .HasForeignKey(cts => cts.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(cts => new { cts.CoderId, cts.SkillId })
            .IsUnique();
    }
}
