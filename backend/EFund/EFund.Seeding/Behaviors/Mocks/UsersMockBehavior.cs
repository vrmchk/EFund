using EFund.Common.Constants;
using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace EFund.Seeding.Behaviors.Mocks;

[DependsOn([typeof(RolesSeedingBehavior)])]
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

            await _userManager.CreateAsync(user, GeneralConfig.MockedUsersPassword);
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
            CreatedAt = DateTimeOffset.Now.AddYears(-5),
        }, Roles.Admin),
        (new User
        {
            DisplayName = "Ihor",
            Email = "ihverwork@gmail.com",
            UserName = "ihverwork@gmail.com",
            EmailConfirmed = true,
            CreatedAt = DateTimeOffset.Now.AddYears(-2),
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
            CreatedAt = DateTimeOffset.Now.AddYears(-2),
            UserMonobanks =
            [
                new UserMonobank
                {
                    MonobankToken =
                        Convert.FromHexString(
                            "68D6A8FC2A4E5887AF0366852CC1B4DFC9B76FA7175EC76676FF5A27E4D771A5CB75F6AA96C87F0EC87D4B9F8FCE1BE4E7D29DBACDFE0A7B42AE72B82A545833"),
                }
            ],
        }, Roles.User),
        (new User
        {
            DisplayName = "Bombardiro Crocdilo",
            Email = "croco@gmail.com",
            UserName = "croco@gmail.com",
            EmailConfirmed = true,
            CreatedAt = DateTimeOffset.Now.AddYears(-3),
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
            DisplayName = "Tralalero Tralala",
            Email = "tralala@gmail.com",
            UserName = "tralala@gmail.com",
            EmailConfirmed = true,
            CreatedAt = DateTimeOffset.Now.AddYears(-1),
            UserMonobanks =
            [
                new UserMonobank
                {
                    MonobankToken =
                        Convert.FromHexString(
                            "68D6A8FC2A4E5887AF0366852CC1B4DFC9B76FA7175EC76676FF5A27E4D771A5CB75F6AA96C87F0EC87D4B9F8FCE1BE4E7D29DBACDFE0A7B42AE72B82A545833"),
                }
            ],
        }, Roles.User)
    ];
}