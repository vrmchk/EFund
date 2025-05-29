using EFund.Common.Models.Configs;
using EFund.Seeding.Behaviors.Abstractions;

namespace EFund.Seeding.Behaviors.Mocks.Abstractions;

public abstract class BaseMockBehavior(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig
)
    : BaseSeedingBehavior(appDataConfig)
{
    private readonly GeneralConfig _generalConfig = generalConfig;

    public override async Task SeedAsync()
    {
        if (!_generalConfig.IsMockingEnabled)
            return;

        await MockData();
    }

    protected abstract Task MockData();
}