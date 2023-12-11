using EFund.Common.Models.Configs.Abstract;
using FluentValidation;
using FluentValidation.Results;

namespace EFund.Validation.Utility;

public abstract class ConfigValidatorBase<T> : AbstractValidator<T>, IConfigValidator
    where T : ConfigBase
{
    public ValidationResult ValidateConfig(ConfigBase config)
    {
        return config is not T configT
            ? new ValidationResult(new[] { new ValidationFailure("Config", $"Config is not of type {typeof(T).Name}") })
            : Validate(configT);
    }

    public async Task<ValidationResult> ValidateConfigAsync(ConfigBase config)
    {
        return config is not T configT
            ? new ValidationResult(new[] { new ValidationFailure("Config", $"Config is not of type {typeof(T).Name}") })
            : await ValidateAsync(configT);
    }
}