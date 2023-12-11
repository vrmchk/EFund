using EFund.Common.Models.Configs;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class AppDataConfigValidator : ConfigValidatorBase<AppDataConfig>
{
    public AppDataConfigValidator()
    {
        RuleFor(x => x.AppDataPath)
            .NotEmpty();

        RuleFor(x => x.LogDirectory)
            .NotEmpty();
    }
}