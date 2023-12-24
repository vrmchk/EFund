using EFund.Common.Models.Configs;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class CacheConfigValidator : ConfigValidatorBase<CacheConfig>
{
    public CacheConfigValidator()
    {
        RuleFor(x => x.SlidingLifetime)
            .NotEmpty();

        RuleFor(x => x.AbsoluteLifetime)
            .NotEmpty();
    }
}