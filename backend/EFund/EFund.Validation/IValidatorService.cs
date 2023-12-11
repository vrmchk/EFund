using FluentValidation.Results;

namespace EFund.Validation;

public interface IValidatorService
{
    ValidationResult Validate<T>(T instance);
    Task<ValidationResult> ValidateAsync<T>(T instance, CancellationToken cancellationToken = default);
}