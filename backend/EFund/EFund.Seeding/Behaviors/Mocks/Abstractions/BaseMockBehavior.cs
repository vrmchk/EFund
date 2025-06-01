using EFund.Common.Models.Configs;
using EFund.Seeding.Behaviors.Abstractions;

namespace EFund.Seeding.Behaviors.Mocks.Abstractions;

public abstract class BaseMockBehavior(
    AppDataConfig appDataConfig,
    GeneralConfig generalConfig
)
    : BaseSeedingBehavior(appDataConfig)
{
    protected readonly GeneralConfig GeneralConfig = generalConfig;
    protected readonly Random Random = new(217);

    public override async Task SeedAsync()
    {
        if (!GeneralConfig.IsMockingEnabled)
            return;

        await MockData();
    }

    protected abstract Task MockData();

    protected T PickRandom<T>(List<T> items)
    {
        return items[Random.Next(items.Count)];
    } 

    protected T PickRandom<T>(T[] items)
    {
        return items[Random.Next(items.Length)];
    }

    protected List<T> PickRandom<T>(List<T> items, int count)
    {
        if (count <= 0 || count > items.Count)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be between 1 and the number of items.");

        var selectedItems = new List<T>();
        var indices = new HashSet<int>();

        while (selectedItems.Count < count)
        {
            var index = Random.Next(items.Count);
            if (indices.Add(index))
            {
                selectedItems.Add(items[index]);
            }
        }

        return selectedItems;
    }
    
    protected List<T> PickRandom<T>(T[] items, int count) => PickRandom(items.ToList(), count);
}