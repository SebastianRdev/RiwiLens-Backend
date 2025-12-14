using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder
            .HasOne(f => f.Coder)
            .WithMany(c => c.Feedback)
            .HasForeignKey(f => f.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(f => f.TeamLeader)
            .WithMany(tl => tl.FeedbackGiven)
            .HasForeignKey(f => f.TeamLeaderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
