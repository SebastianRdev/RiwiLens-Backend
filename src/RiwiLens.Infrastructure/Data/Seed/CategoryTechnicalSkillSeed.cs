using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Infrastructure.Persistence;

namespace src.RiwiLens.Infrastructure.Data.Seed;

public static class CategoryTechnicalSkillSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.CategoryTechnicalSkills.Any()) return;

        var categories = new List<CategoryTechnicalSkill>
        {
            CategoryTechnicalSkill.Create("Frontend"),
            CategoryTechnicalSkill.Create("Backend"),
            CategoryTechnicalSkill.Create("Database"),
            CategoryTechnicalSkill.Create("DevOps"),
            CategoryTechnicalSkill.Create("Cloud"),
            CategoryTechnicalSkill.Create("Testing"),
            CategoryTechnicalSkill.Create("Architecture"),
            CategoryTechnicalSkill.Create("Tools"),
            CategoryTechnicalSkill.Create("Mobile")
        };

        context.CategoryTechnicalSkills.AddRange(categories);
        await context.SaveChangesAsync();
    }
}
