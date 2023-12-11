using EFund.Common.Models.Configs;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;
   
public class JwtConfigValidator : ConfigValidatorBase<JwtConfig>
{
    public JwtConfigValidator()
    {
        RuleFor(x => x.Secret)
            .NotEmpty();

        RuleFor(x => x.Issuer)
            .NotEmpty();

        RuleFor(x => x.Audience)
            .NotEmpty();

        RuleFor(x => x.AccessTokenLifetime)
            .NotEmpty()
            .GreaterThan(TimeSpan.Zero);

        RuleFor(x => x.RefreshTokenLifetime)
            .NotEmpty()
            .GreaterThan(TimeSpan.Zero);
    }
}