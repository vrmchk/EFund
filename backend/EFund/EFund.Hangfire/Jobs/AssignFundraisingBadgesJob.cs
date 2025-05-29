using EFund.Common.Enums;
using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFund.Hangfire.Jobs;

public class AssignFundraisingBadgesJob(
    UserManager<User> userManager,
    IRepository<Fundraising> fundraisingRepository
)
    : IJob<AssignFundraisingBadgesJobArgs>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IRepository<Fundraising> _fundraisingRepository = fundraisingRepository;

    public static string Id => nameof(AssignUserCreatedBadgesJob);

    public async Task Run(AssignFundraisingBadgesJobArgs data, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Include(u => u.Badges)
            .SingleOrDefaultAsync(u => u.Id == data.UserId, cancellationToken);

        if (user == null)
            return;

        var count = await _fundraisingRepository.CountAsync(f => f.UserId == data.UserId && f.Status == FundraisingStatus.Reviewed, cancellationToken);
        var (newType, oldType) = count switch
        {
            1 => (BadgeType.NoviceFundraiser, BadgeType.Freshman),
            >= 3 and < 10 => (BadgeType.IntermediateFundraiser, BadgeType.NoviceFundraiser),
            >= 10 => (BadgeType.ExperiencedFundraiser, BadgeType.IntermediateFundraiser),
            _ => (BadgeType.None, BadgeType.None)
        };

        if (newType == BadgeType.None)
            return;

        user.Badges = user.Badges
            .Where(b => b.Type != oldType)
            .Append(new Badge { Type = newType })
            .ToList();

        await _userManager.UpdateAsync(user);
    }
}