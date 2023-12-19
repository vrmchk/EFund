using EFund.Common.Models.Configs;
using EFund.Validation.Extensions;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class CallbackUrlsConfigValidator : ConfigValidatorBase<CallbackUrisConfig>
{
    public CallbackUrlsConfigValidator()
    {
        RuleFor(x => x.ResetPasswordUriTemplate)
            .NotEmpty()
            .HasFormatParams(2);

        RuleFor(x => x.InviteUserUriTemplate)
            .NotEmpty()
            .HasFormatParams(2);
    }
}