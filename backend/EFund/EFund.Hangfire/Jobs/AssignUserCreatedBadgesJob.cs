using EFund.Common.Enums;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFund.Hangfire.Jobs;

public class AssignUserCreatedBadgesJob(
    UserManager<User> userManager,
    IRepository<Badge> badgeRepository)
    : IJob<AssignUserCreatedBadgesJobArgs>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IRepository<Badge> _badgeRepository = badgeRepository;
    private readonly BadgeType[] _badgeTypesToAssign = [BadgeType.Freshman, BadgeType.UserSince];

    public static string Id => nameof(AssignUserCreatedBadgesJob);

    public async Task Run(AssignUserCreatedBadgesJobArgs data, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(data.UserId.ToString());
        if (user == null)
            return;

        var badges = await _badgeRepository.Where(b => _badgeTypesToAssign.Contains(b.Type)).ToListAsync(cancellationToken);

        user.Badges = badges;
        await _userManager.UpdateAsync(user);
    }
}