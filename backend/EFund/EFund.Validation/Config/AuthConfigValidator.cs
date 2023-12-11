using EFund.Common.Models.Configs;
using EFund.Validation.Extensions;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class AuthConfigValidator : ConfigValidatorBase<AuthConfig>
{
    public AuthConfigValidator()
    {
        RuleFor(x => x.ConfirmationCodeLenght)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.ConfirmationCodeLifetime)
            .NotEmpty()
            .GreaterThan(TimeSpan.Zero);

        RuleFor(x => x.ResetPasswordUriTemplate)
            .NotEmpty()
            .HasFormatParams(2);
    }
}