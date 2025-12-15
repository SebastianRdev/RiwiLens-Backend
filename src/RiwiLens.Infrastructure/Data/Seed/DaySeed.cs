using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Infrastructure.Persistence;

namespace src.RiwiLens.Infrastructure.Data.Seed;

public static class DaySeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Days.Any()) return;

        var days = new List<Day>
        {
            Day.Create("Morning", new TimeSpan(7, 0, 0), new TimeSpan(12, 0, 0)),
            Day.Create("Afternoon", new TimeSpan(13, 0, 0), new TimeSpan(18, 0, 0))
        };

        context.Days.AddRange(days);
        await context.SaveChangesAsync();
    }
}
