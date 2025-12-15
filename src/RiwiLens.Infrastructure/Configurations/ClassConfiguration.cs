using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder
            .HasOne(c => c.TeamLeader)
            .WithMany(tl => tl.Classes)
            .HasForeignKey(c => c.TeamLeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.Day)
            .WithMany(d => d.Classes)
            .HasForeignKey(c => c.DayId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.ClassType)
            .WithMany(ct => ct.Classes)
            .HasForeignKey(c => c.ClassTypeId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}
