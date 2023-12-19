namespace EFund.Seeding.Behaviors.Abstractions;

public abstract class BaseSeedingBehavior : ISeedingBehavior
{
    protected string DataFolder { get; } = $"{Path.GetDirectoryName(typeof(BaseSeedingBehavior).Assembly.Location)}/Data";

    public abstract Task SeedAsync();

}
