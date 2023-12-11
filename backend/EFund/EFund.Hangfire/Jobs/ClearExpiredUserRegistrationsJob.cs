using EFund.BLL.Services.Interfaces;
using EFund.Hangfire.Abstractions;

namespace EFund.Hangfire.Jobs;

public class ClearExpiredUserRegistrationsJob : IJob
{
    private readonly IUserCleanerService _userCleanerService;

    public ClearExpiredUserRegistrationsJob(IUserCleanerService userCleanerService)
    {
        _userCleanerService = userCleanerService;
    }

    public static string Id => nameof(ClearExpiredUserRegistrationsJob);

    public async Task Run(CancellationToken cancellationToken = default)
    {
        await _userCleanerService.ClearExpiredUsersAsync();
    }
}