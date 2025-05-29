using System.Reflection;

namespace EFund.Common.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<Type> GetTypesImplementingInterface<TInterface>(this Assembly assembly)
    {
        var interfaceType = typeof(TInterface);

        return assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false } && interfaceType.IsAssignableFrom(t));
    }
}