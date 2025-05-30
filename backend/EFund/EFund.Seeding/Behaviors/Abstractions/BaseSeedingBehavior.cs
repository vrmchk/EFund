using EFund.Common.Models.Configs;

namespace EFund.Seeding.Behaviors.Abstractions;

public abstract class BaseSeedingBehavior : ISeedingBehavior
{
    protected readonly AppDataConfig AppDataConfig;

    protected BaseSeedingBehavior(AppDataConfig appDataConfig)
    {
        AppDataConfig = appDataConfig;
    }

    protected string DataFolder => AppDataConfig.SeedingDataPath;

    public abstract Task SeedAsync();
}