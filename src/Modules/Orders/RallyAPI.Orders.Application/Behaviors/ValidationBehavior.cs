using FluentValidation;
using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior for automatic validation.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
        {
            var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));

            // Return failure result if TResponse is Result type
            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var failureMethod = typeof(Result)
                    .GetMethods()
                    .First(m => m.Name == "Failure" && m.IsGenericMethod)
                    .MakeGenericMethod(resultType);

                var error = Error.Validation(errorMessage);
                return (TResponse)failureMethod.Invoke(null, new object[] { error })!;
            }

            throw new ValidationException(failures);
        }

        return await next();
    }
}