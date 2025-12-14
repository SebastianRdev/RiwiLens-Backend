using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class CoderSoftSkillConfiguration : IEntityTypeConfiguration<CoderSoftSkill>
{
    public void Configure(EntityTypeBuilder<CoderSoftSkill> builder)
    {
        builder
            .HasOne(cs => cs.Coder)
            .WithMany(c => c.SoftSkills)
            .HasForeignKey(cs => cs.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(cs => cs.SoftSkill)
            .WithMany(ss => ss.CoderSoftSkills)
            .HasForeignKey(cs => cs.SoftSkillId)
            .OnDelete(DeleteBehavior.Restrict);

        // A coder cannot have the same soft skill twice
        builder
            .HasIndex(cs => new { cs.CoderId, cs.SoftSkillId })
            .IsUnique();
    }
}
