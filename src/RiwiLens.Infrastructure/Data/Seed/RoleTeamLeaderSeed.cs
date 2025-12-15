using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Infrastructure.Persistence;

namespace src.RiwiLens.Infrastructure.Data.Seed;

public static class RoleTeamLeaderSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.RoleTeamLeaders.Any()) return;

        context.RoleTeamLeaders.AddRange(
            new RoleTeamLeader("Development", "Technical development mentor"),
            new RoleTeamLeader("English", "English language mentor")
        );

        await context.SaveChangesAsync();
    }
}
