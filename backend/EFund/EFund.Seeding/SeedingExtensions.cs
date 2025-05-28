using EFund.Seeding.Behaviors;
using EFund.Seeding.Behaviors.Abstractions;
using EFund.Seeding.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EFund.Seeding;

public static class SeedingExtensions
{
    public static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        services.AddScoped<ISeedingBehavior, RolesSeedingBehavior>();
        services.AddScoped<ISeedingBehavior, ViolationGroupsSeedingBehavior>();
        services.AddScoped<ISeedingBehavior, ViolationsSeedingBehavior>();

        return services;
    }

    public static async Task SeedDataAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var behaviors = services.GetRequiredService<IEnumerable<ISeedingBehavior>>();

        foreach (var behavior in behaviors.OrderBy(b => b.GetType().GetDepth()))
        {
            await behavior.SeedAsync();
        }
    }
}
