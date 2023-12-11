using EFund.Common.Models.Configs;
using EFund.Hangfire.Abstractions;
using EFund.Hangfire.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EFund.Hangfire;

public static class HangfireExtensions
{
    public static void SetupHangfire(this IHost app, HangfireConfig config)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var hangfireService = services.GetRequiredService<IHangfireService>();

        hangfireService.SetupRecurring<ClearExpiredUserRegistrationsJob>(
            ClearExpiredUserRegistrationsJob.Id,
            config.ClearExpiredUserRegistrationsCron);
    }
}