using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EFund.BLL.Extensions;

public static class EnumExtensions
{
    public static T ToEnum<T>(this string source) where T : struct, Enum
    {
        return Enum.TryParse(source, true, out T result) ? result : default;
    }
    
    public static string GetDisplayName(this Enum value)
    {
        return value.GetAttribute<DisplayAttribute>()?.Name ?? value.ToString();
    }

    public static string GetDescription(this Enum value)
    {
        return value.GetAttribute<DisplayAttribute>()?.Description ?? string.Empty;
    }

    private static TAttribute? GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        return memberInfo?.GetCustomAttribute<TAttribute>();
    }
}