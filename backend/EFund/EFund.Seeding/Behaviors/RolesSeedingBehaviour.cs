using EFund.Common.Constants;
using EFund.Common.Models.Configs;
using EFund.Seeding.Behaviors.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace EFund.Seeding.Behaviors;

public class RolesSeedingBehaviour : BaseSeedingBehavior
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RolesSeedingBehaviour(AppDataConfig appDataConfig, RoleManager<IdentityRole<Guid>> roleManager) 
        : base(appDataConfig)
    {
        _roleManager = roleManager;
    }

    public override async Task SeedAsync()
    {
        var roles = new List<string> { Roles.Admin, Roles.User };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
}