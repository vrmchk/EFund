using EFund.Common.Enums;

namespace EFund.BLL.Extensions;

public static class DateTimeExtensions
{
    public static DateTimeOffset UnixSecondsToDateTimeOffset(this long unix)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unix);
    }

    public static DateTime UnixSecondsToDateTime(this long unix)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unix).DateTime;
    }

    public static bool IsMonthYearEqual(this DateTime date, DateTime dateTime)
    {
        return date.Month == dateTime.Month && date.Year == dateTime.Year;
    }

    public static long ToUnixSeconds(this DateOnly dateOnly, TimeOfDay timeOfDay)
    {
        var time = timeOfDay == TimeOfDay.Min ? TimeOnly.MinValue : TimeOnly.MaxValue;
        return ((DateTimeOffset)dateOnly.ToDateTime(time)).ToUnixTimeSeconds();
    }

    public static DateTime ToDateTime(this DateOnly dateOnly)
    {
        return dateOnly.ToDateTime(TimeOnly.MinValue);
    }

    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }
}