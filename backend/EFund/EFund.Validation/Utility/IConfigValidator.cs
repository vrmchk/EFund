using EFund.Common.Models.Configs.Abstract;
using FluentValidation;
using FluentValidation.Results;

namespace EFund.Validation.Utility;

public interface IConfigValidator : IValidator
{
    ValidationResult ValidateConfig(ConfigBase config);
    Task<ValidationResult> ValidateConfigAsync(ConfigBase config);
}