using EFund.DAL.Entities;
using EFund.DAL.Repositories.Interfaces;
using EFund.Hangfire.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFund.Hangfire.Jobs;

public class ClearExpiredUserRegistrationsJob(
    IRepository<UserRegistration> userRegistrationRepository,
    UserManager<User> userManager
) : IJob
{
    private readonly IRepository<UserRegistration> _userRegistrationRepository = userRegistrationRepository;
    private readonly UserManager<User> _userManager = userManager;

    public static string Id => nameof(ClearExpiredUserRegistrationsJob);

    public async Task Run(CancellationToken cancellationToken = default)
    {
        var expiredUsers = _userRegistrationRepository.Where(user =>
            user.IsCodeRegenerated && user.ExpiresAt <= DateTimeOffset.UtcNow);

        var users = await expiredUsers.Include(ur => ur.User).Select(ur => ur.User).ToListAsync(cancellationToken);
        foreach (var user in users)
        {
            await _userManager.DeleteAsync(user);
        }

        await _userRegistrationRepository.DeleteManyAsync(await expiredUsers.ToListAsync(cancellationToken));
    }
}