using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder
            .HasOne(a => a.Coder)
            .WithMany(c => c.Attendances)
            .HasForeignKey(a => a.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(a => a.Class)
            .WithMany()
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(a => a.Clan)
            .WithMany()
            .HasForeignKey(a => a.ClanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

