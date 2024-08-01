using Client.Features.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Server.Features.Auth;

public class RoleSeeder(RoleManager<IdentityRole<int>> roleManager, ILogger<RoleSeeder> logger)
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        HashSet<string> existingRoleNames =
        [
            ..await roleManager.Roles.Select(x => x.Name!).ToArrayAsync(ct)
        ];

        var rolesToAdd = RoleNames.Enumerate()
            .Where(roleName => existingRoleNames.Contains(roleName) is false)
            .Select(roleName => new IdentityRole<int>(roleName));

        foreach (var role in rolesToAdd)
        {
            await roleManager.CreateAsync(role);
        }

        logger.LogInformation("Roles seeded");
    }
}