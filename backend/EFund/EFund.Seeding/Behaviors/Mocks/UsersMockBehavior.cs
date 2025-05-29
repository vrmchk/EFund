using EFund.Common.Constants;
using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace EFund.Seeding.Behaviors.Mocks;

public class UsersMockBehavior(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig,
    UserManager<User> userManager)
    : BaseMockBehavior(appDataConfig, generalConfig)
{
    private readonly UserManager<User> _userManager = userManager;

    protected override async Task MockData()
    {
        foreach (var (user, role) in Users)
        {
            if (await _userManager.FindByEmailAsync(user.Email!) != null)
                continue;

            await _userManager.CreateAsync(user, "Qwerty.123");
            await _userManager.AddToRoleAsync(user, role);
        }
    }

    private (User User, string Role)[] Users { get; } =
    [
        (new User
        {
            DisplayName = "Admin",
            Email = "admin@admin.com",
            UserName = "admin@admin.com",
            EmailConfirmed = true,
            CreatedAt = DateTimeOffset.Now,
        }, Roles.Admin),
        (new User
        {
            DisplayName = "Ihor",
            Email = "ihverwork@gmail.com",
            UserName = "ihverwork@gmail.com",
            EmailConfirmed = true,
            CreatedAt = DateTimeOffset.Now,
            UserMonobanks =
            [
                new UserMonobank
                {
                    MonobankToken =
                        Convert.FromHexString(
                            "3B35663CFB669144DF4650095A8C004933564CED8600E51BFD17EFD915259BF0DD134A3960C1BF4645B0CD1862B75A53B4E6E6B72136C23A6FAE1BBB39AEAF16"),
                }
            ],
        }, Roles.User),
        (new User
        {
            DisplayName = "Vlad",
            Email = "vladd.golovatyuk@gmail.com",
            UserName = "vladd.golovatyuk@gmail.com",
            EmailConfirmed = true,
            CreatedAt = DateTimeOffset.Now,
            UserMonobanks =
            [
                new UserMonobank
                {
                    MonobankToken =
                        Convert.FromHexString(
                            "92ABFCEDD31F7CDE7D8EF02187E91C272B786B347EA7C660659BA9E2DC5E41F300FA31C72F1EE35AF770D6F926101EA2C15226B73CF6F565A4BCC19E984D872F"),
                }
            ],
        }, Roles.User)
    ];
}