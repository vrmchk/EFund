using EFund.Common.Models.Configs;
using EFund.Validation.Extensions;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class HangfireConfigValidator : ConfigValidatorBase<HangfireConfig>
{
    public HangfireConfigValidator()
    {
        RuleFor(x => x.User)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.ClearExpiredUserRegistrationsCron)
            .NotEmpty()
            .CronExpression();
    }
}