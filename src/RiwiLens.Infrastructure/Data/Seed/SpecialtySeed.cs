using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Infrastructure.Persistence;

namespace src.RiwiLens.Infrastructure.Data.Seed;

public static class SpecialtySeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Specialties.Any()) return;

        var specialties = new List<Specialty>
        {
            Specialty.Create("Backend Development", "APIs, architecture, performance"),
            Specialty.Create("Frontend Development", "UI, UX, accessibility"),
            Specialty.Create("DevOps", "CI/CD, infrastructure, automation"),
            Specialty.Create("Soft Skills Coaching", "Communication and leadership"),
            Specialty.Create("Public Speaking", "Presentation and communication skills")
        };

        context.Specialties.AddRange(specialties);
        await context.SaveChangesAsync();
    }
}
