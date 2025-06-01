using EFund.Common.Models.Configs;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class GeneralConfigValidator : ConfigValidatorBase<GeneralConfig>
{
    public GeneralConfigValidator()
    {
        RuleFor(x => x.MockedUsersPassword)
            .NotEmpty();
    }
}