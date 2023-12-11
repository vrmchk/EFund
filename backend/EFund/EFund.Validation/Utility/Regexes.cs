namespace EFund.Validation.Utility;

public static class Regexes
{
    public const string Password = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&\.,:;])[A-Za-z\d@$!%*#?&\.,:;]+$";
    public const string ColorHash = @"^#(?:[0-9a-fA-F]{3}){1,2}$";
    public const string Cron = @"^(((\d+,)+\d+|(\d+(\/|-)\d+)|\d+|\*) ?){5,7}$";
    public const string MonthYear = @"^((0?\d)|(10|11|12))\/\d{2,4}$";

    public static string HasFormatParams(int amount)
    {
        return @$"^(?!(.*\{{\d\}}.*){{{amount + 1}}})(.*\{{\d\}}.*){{{amount}}}$";
    }
}