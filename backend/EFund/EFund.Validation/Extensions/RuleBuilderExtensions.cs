using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> IsEnum<T>(this IRuleBuilder<T, string> ruleBuilder, Type enumType,
        bool caseSensitive = false)
    {
        return ruleBuilder
            .IsEnumName(enumType, caseSensitive)
            .WithMessage($"Not valid enum name. Possible values: {string.Join(", ", Enum.GetNames(enumType))}");
    }

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder,
        int minimumLength = 8)
    {
        return ruleBuilder
            .MinimumLength(minimumLength)
            .Matches(Regexes.Password)
            .WithMessage(
                "Password must contain at least one uppercase letter, one lowercase letter, one digit and one special character");
    }

    public static IRuleBuilderOptions<T, string> ColorHash<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(Regexes.ColorHash)
            .WithMessage("Color hash is invalid");
    }

    public static IRuleBuilderOptions<T, string> CronExpression<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(Regexes.Cron)
            .WithMessage("Cron expression is invalid");
    }

    public static IRuleBuilderOptions<T, string> HasFormatParams<T>(this IRuleBuilder<T, string> ruleBuilder,
        int amount)
    {
        return ruleBuilder
            .Matches(Regexes.HasFormatParams(amount))
            .WithMessage("Number of format parameters is invalid");
    }

    public static IRuleBuilderOptions<T, string> MonthYear<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Matches(Regexes.MonthYear);
    }
}