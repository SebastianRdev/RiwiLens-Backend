using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class TechnicalSkillConfiguration 
    : IEntityTypeConfiguration<TechnicalSkill>
{
    public void Configure(EntityTypeBuilder<TechnicalSkill> builder)
    {
        builder
            .HasOne(ts => ts.Category)
            .WithMany(c => c.TechnicalSkills)
            .HasForeignKey(ts => ts.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(ts => ts.CoderTechnicalSkills)
            .WithOne(cts => cts.TechnicalSkill)
            .HasForeignKey(cts => cts.SkillId);
    }
}
