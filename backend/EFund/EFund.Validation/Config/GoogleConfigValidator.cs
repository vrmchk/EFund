using EFund.Common.Models.Configs;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class GoogleConfigValidator : ConfigValidatorBase<GoogleConfig>
{
    public GoogleConfigValidator()
    {
        RuleFor(x => x.ClientSecret)
            .NotEmpty();

        RuleFor(x => x.ClientId)
            .NotEmpty();

        RuleFor(x => x.RedirectUri)
            .NotEmpty();

        RuleFor(x => x.ClockTolerance)
            .NotEmpty();
    }
}