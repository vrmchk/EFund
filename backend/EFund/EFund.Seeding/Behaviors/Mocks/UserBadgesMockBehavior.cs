using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Behaviors.Mocks.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFund.Seeding.Behaviors.Mocks;

[DependsOn([typeof(BadgesSeedingBehavior), typeof(UsersMockBehavior)])]
public class UserBadgesMockBehavior(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig,
    UserManager<User> userManager,
    IRepository<Badge> badgeRepository)
    : BaseMockBehavior(appDataConfig, generalConfig)
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IRepository<Badge> _badgeRepository = badgeRepository;

    private readonly Dictionary<string, BadgeType[]> _userBadges = new()
    {
        ["admin@admin.com"] = [BadgeType.UserSince, BadgeType.Freshman],
        ["vladd.golovatyuk@gmail.com"] = [BadgeType.UserSince, BadgeType.ExperiencedFundraiser],
        ["ihverwork@gmail.com"] = [BadgeType.UserSince, BadgeType.IntermediateFundraiser],
    };

    protected override async Task MockData()
    {
        var users = await _userManager.Users
            .Where(u => _userBadges.Keys.Contains(u.Email!))
            .Include(u => u.Badges)
            .ToListAsync();

        foreach (var user in users)
        {
            if (user.Badges.Count > 0)
                continue;

            var badgeTypesToAssign = _userBadges[user.Email!];
            var badges = await _badgeRepository.Where(b => badgeTypesToAssign.Contains(b.Type)).ToListAsync();
            user.Badges = badges;
            await _userManager.UpdateAsync(user);
        }
    }
}