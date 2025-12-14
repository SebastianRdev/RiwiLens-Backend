using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Infrastructure.Identity;

namespace src.RiwiLens.Infrastructure.Persistence;

public class ApplicationDbContext 
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // MAIN ENTITIES
    public DbSet<Coder> Coders => Set<Coder>();
    public DbSet<TeamLeader> TeamLeaders => Set<TeamLeader>();

    // RELATIONS
    public DbSet<CoderSoftSkill> CoderSoftSkills => Set<CoderSoftSkill>();
    public DbSet<CoderTechnicalSkill> CoderTechnicalSkills => Set<CoderTechnicalSkill>();
    public DbSet<TeamLeaderSpecialty> TeamLeaderSpecialties => Set<TeamLeaderSpecialty>();
    public DbSet<ClanCoder> ClanCoders => Set<ClanCoder>();
    public DbSet<ClanTeamLeader> ClanTeamLeaders => Set<ClanTeamLeader>();

    // ATTENDANCE AND BIOMETRICS
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<FaceCollection> FaceCollections => Set<FaceCollection>();

    // CATALOGUES
    public DbSet<Clan> Clans => Set<Clan>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<ClassType> ClassTypes => Set<ClassType>();
    public DbSet<Day> Days => Set<Day>();
    public DbSet<SoftSkill> SoftSkills => Set<SoftSkill>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<RoleTeamLeader> RoleTeamLeaders => Set<RoleTeamLeader>();
    public DbSet<TechnicalSkill> TechnicalSkills => Set<TechnicalSkill>();
    public DbSet<CategoryTechnicalSkill> CategoryTechnicalSkills => Set<CategoryTechnicalSkill>();
    public DbSet<StatusCoder> StatusCoders => Set<StatusCoder>();

    // OTHERS
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<ProfessionalProfile> ProfessionalProfiles => Set<ProfessionalProfile>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Automatic loading of all Fluent Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );
    }
}
