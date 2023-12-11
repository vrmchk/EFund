using EFund.Common.Models.Configs;
using EFund.Validation.Utility;
using FluentValidation;

namespace EFund.Validation.Config;

public class EncryptionConfigValidator : ConfigValidatorBase<EncryptionConfig>
{
    public EncryptionConfigValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty();
    }
}