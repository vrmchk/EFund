using EFund.Common.Enums;
using EFund.DAL.Entities;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;
using Microsoft.AspNetCore.Identity;

namespace EFund.Hangfire.Jobs;

public class AssignUserCreatedBadgesJob : IJob<AssignUserCreatedBadgesJobArgs>
{
    private readonly UserManager<User> _userManager;
    private readonly BadgeType[] _badgeTypesToAssign = [BadgeType.Freshman, BadgeType.UserSince];

    public AssignUserCreatedBadgesJob(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public static string Id => nameof(AssignUserCreatedBadgesJob);

    public async Task Run(AssignUserCreatedBadgesJobArgs data, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(data.UserId.ToString());
        if (user == null)
            return;

        user.Badges = _badgeTypesToAssign.Select(b => new Badge
        {
            Type = b
        }).ToList();

        await _userManager.UpdateAsync(user);
    }
}