using EFund.Common.Models.Configs;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class MonobankConfigValidator : ConfigValidatorBase<MonobankConfig>
{
    public MonobankConfigValidator()
    {
        RuleFor(x => x.HttpClientName)
            .NotEmpty();

        RuleFor(x => x.BaseAddress)
            .NotEmpty();
    }
}