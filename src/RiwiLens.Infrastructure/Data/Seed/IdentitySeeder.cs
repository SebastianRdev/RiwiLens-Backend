using src.RiwiLens.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace src.RiwiLens.Infrastructure.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = { "Admin", "TeamLeader", "Coder" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }
    }
}