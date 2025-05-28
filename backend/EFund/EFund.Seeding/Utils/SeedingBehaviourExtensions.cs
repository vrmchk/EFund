using System.Reflection;
using EFund.Seeding.Behaviors.Abstractions;

namespace EFund.Seeding.Utils;

public static class SeedingBehaviourExtensions
{
    public static int GetDepth(this Type type)
    {
        if (!type.IsAssignableTo(typeof(ISeedingBehavior)))
            throw new ArgumentException(nameof(Type));

        var depth = 0;
        return GetDepthRecursive(type, ref depth);
    }

    private static int GetDepthRecursive(Type type, ref int depth)
    {
        var dependsOn = type.GetCustomAttribute<DependsOnAttribute>();
        if (dependsOn == null || dependsOn.Behaviors.Length == 0)
            return depth;

        var tmp = depth;
        return dependsOn.Behaviors.Select(b =>
        {
            var currentDepth = tmp + 1;
            return GetDepthRecursive(b, ref currentDepth);
        }).Max();
    }
}