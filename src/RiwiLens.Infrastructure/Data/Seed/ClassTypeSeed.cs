using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Infrastructure.Persistence;

namespace src.RiwiLens.Infrastructure.Data.Seed;

public static class ClassTypeSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.ClassTypes.Any()) return;

        var classTypes = new List<ClassType>
        {
            ClassType.Create("Development"),
            ClassType.Create("English")
        };

        context.ClassTypes.AddRange(classTypes);
        await context.SaveChangesAsync();
    }
}
