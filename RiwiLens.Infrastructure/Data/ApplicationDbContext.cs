using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RiwiLens.Domain.Entities;

namespace RiwiLens.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ENTIDADES PRINCIPALES
    public DbSet<Coder> Coders => Set<Coder>();
    public DbSet<TeamLeader> TeamLeaders => Set<TeamLeader>();

    // RELACIONADAS
    public DbSet<CoderTechnicalSkill> CoderTechnicalSkills => Set<CoderTechnicalSkill>();
    public DbSet<TeamLeaderSpecialty> TeamLeaderSpecialties => Set<TeamLeaderSpecialty>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<FaceCollection> FaceCollections => Set<FaceCollection>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Coder → Technical Skills (1-M)
        modelBuilder.Entity<Coder>()
            .HasMany(c => c.TechnicalSkills)
            .WithOne(ts => ts.Coder)
            .HasForeignKey(ts => ts.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        // TeamLeader → Specialties (1-M)
        modelBuilder.Entity<TeamLeader>()
            .HasMany(t => t.TeamLeaderSpecialties)
            .WithOne(s => s.TeamLeader)
            .HasForeignKey(s => s.TeamLeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Attendance → Coder (N-1)
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Coder)
            .WithMany(c => c.Attendances)
            .HasForeignKey(a => a.CoderId)
            .OnDelete(DeleteBehavior.Cascade);

        // FaceCollection → Coder (1-Coder, N-Faces)
        modelBuilder.Entity<FaceCollection>()
            .HasOne(f => f.Coder)
            .WithMany(c => c.FaceCollections)
            .HasForeignKey(f => f.CoderId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}