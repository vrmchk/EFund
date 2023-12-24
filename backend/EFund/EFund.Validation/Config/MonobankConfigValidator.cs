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
        
        RuleFor(x => x.SendAddress)
            .NotEmpty();

        RuleFor(x => x.ClientInfoCacheSlidingLifetime)
            .NotEmpty();

        RuleFor(x => x.ClientInfoCacheAbsoluteLifetime)
            .NotEmpty();

        RuleFor(x => x.ClientInfoCacheBackupSlidingLifetime)
            .NotEmpty();

        RuleFor(x => x.ClientInfoCacheBackupAbsoluteLifetime)
            .NotEmpty();
    }
}