using EFund.Common.Models.Configs;

namespace EFund.Seeding.Behaviors.Abstractions;

public abstract class BaseSeedingBehavior : ISeedingBehavior
{
    private readonly AppDataConfig _appDataConfig;

    protected BaseSeedingBehavior(AppDataConfig appDataConfig)
    {
        _appDataConfig = appDataConfig;
    }

    protected string DataFolder => _appDataConfig.SeedingDataPath;

    public abstract Task SeedAsync();
}