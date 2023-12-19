namespace EFund.BLL.Extensions;

public static class EnumExtensions
{
    public static T ToEnum<T>(this string source) where T : struct, Enum
    {
        return Enum.TryParse(source, true, out T result) ? result : default;
    }
}