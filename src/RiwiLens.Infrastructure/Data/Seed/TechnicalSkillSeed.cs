using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Infrastructure.Persistence;

namespace src.RiwiLens.Infrastructure.Data.Seed;

public static class TechnicalSkillSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.TechnicalSkills.Any()) return;

        // Categories (assumed already seeded)
        var frontendId = context.CategoryTechnicalSkills.First(c => c.Name == "Frontend").Id;
        var backendId  = context.CategoryTechnicalSkills.First(c => c.Name == "Backend").Id;
        var databaseId = context.CategoryTechnicalSkills.First(c => c.Name == "Database").Id;
        var devopsId   = context.CategoryTechnicalSkills.First(c => c.Name == "DevOps").Id;

        var skills = new List<TechnicalSkill>
        {
            // Frontend
            TechnicalSkill.Create("HTML", frontendId),
            TechnicalSkill.Create("CSS", frontendId),
            TechnicalSkill.Create("JavaScript", frontendId),
            TechnicalSkill.Create("TypeScript", frontendId),
            TechnicalSkill.Create("React", frontendId),
            TechnicalSkill.Create("Angular", frontendId),
            TechnicalSkill.Create("Vue.js", frontendId),
            TechnicalSkill.Create("Next.js", frontendId),
            TechnicalSkill.Create("Tailwind CSS", frontendId),
            TechnicalSkill.Create("Sass", frontendId),
            TechnicalSkill.Create("Vite", frontendId),
            TechnicalSkill.Create("Webpack", frontendId),

            // Backend
            TechnicalSkill.Create("C#", backendId),
            TechnicalSkill.Create(".NET", backendId),
            TechnicalSkill.Create("ASP.NET Core", backendId),
            TechnicalSkill.Create("Java", backendId),
            TechnicalSkill.Create("Spring Boot", backendId),
            TechnicalSkill.Create("Node.js", backendId),
            TechnicalSkill.Create("Express.js", backendId),
            TechnicalSkill.Create("NestJS", backendId),
            TechnicalSkill.Create("Python", backendId),
            TechnicalSkill.Create("Django", backendId),
            TechnicalSkill.Create("FastAPI", backendId),
            TechnicalSkill.Create("PHP", backendId),
            TechnicalSkill.Create("Laravel", backendId),

            // Database
            TechnicalSkill.Create("PostgreSQL", databaseId),
            TechnicalSkill.Create("MySQL", databaseId),
            TechnicalSkill.Create("SQL Server", databaseId),
            TechnicalSkill.Create("MongoDB", databaseId),
            TechnicalSkill.Create("Redis", databaseId),
            TechnicalSkill.Create("SQLite", databaseId),
            TechnicalSkill.Create("Firebase", databaseId),

            // DevOps
            TechnicalSkill.Create("Docker", devopsId),
            TechnicalSkill.Create("Docker Compose", devopsId),
            TechnicalSkill.Create("Kubernetes", devopsId),
            TechnicalSkill.Create("Git", devopsId),
            TechnicalSkill.Create("GitHub Actions", devopsId),
            TechnicalSkill.Create("GitLab CI", devopsId),
            TechnicalSkill.Create("Linux", devopsId),
            TechnicalSkill.Create("Nginx", devopsId),
            TechnicalSkill.Create("AWS", devopsId),
            TechnicalSkill.Create("Azure", devopsId),
            TechnicalSkill.Create("CI/CD", devopsId)
        };

        context.TechnicalSkills.AddRange(skills);
        await context.SaveChangesAsync();
    }
}
