using System.Collections.Concurrent;
using FluentValidation;
using FluentValidation.Results;

namespace EFund.Validation;

public class ValidatorService : IValidatorService
{
    private readonly ConcurrentDictionary<Type, IValidator> _typeValidators;

    public ValidatorService(IEnumerable<IValidator> validators)
    {
        _typeValidators = new ConcurrentDictionary<Type, IValidator>(
            validators.ToDictionary(v => v.GetType().BaseType!.GenericTypeArguments.First(), v => v));
    }

    public ValidationResult Validate<T>(T instance)
    {
        return GetValidator<T>().Validate(instance);
    }

    public Task<ValidationResult> ValidateAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        return GetValidator<T>().ValidateAsync(instance, cancellationToken);
    }

    private IValidator<T> GetValidator<T>()
    {
        var type = typeof(T);
        if (!_typeValidators.TryGetValue(type, out var validator))
            throw new InvalidOperationException($"No validator found for type {type.Name}.");

        return (IValidator<T>)validator;
    }
}