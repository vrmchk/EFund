using EFund.Common.Models.Configs;
using EFund.DAL.Entities;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.JobArgs;
using Microsoft.AspNetCore.Identity;

namespace EFund.Hangfire.Jobs;

public class UpdateUserRatingJob(
    UserManager<User> userManager, 
    GeneralConfig config
) 
    : IJob<UpdateUserRatingJobArgs>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly GeneralConfig _config = config;

    public static string Id => nameof(UpdateUserRatingJob);

    public async Task Run(UpdateUserRatingJobArgs data, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(data.UserId.ToString());
        if (user == null)
            return;

        user.Rating = Math.Clamp(user.Rating + data.RatingChange, -_config.UserRatingLimit, _config.UserRatingLimit);
        await _userManager.UpdateAsync(user);
    }
}