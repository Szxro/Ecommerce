using Ecommerce.SharedKernel.Common.Primitives;
using FluentValidation.Results;

namespace Ecommerce.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public Error[] Failures { get; }

    public ValidationException(ValidationFailure[] failures)
        : base("One or more validations errors happen")
    {
        Failures = CreateValidationError(failures);
    }

    private static Error[] CreateValidationError(IEnumerable<ValidationFailure> failures)
           => failures.Select(x => Error.Validation(x.ErrorMessage)).ToArray();
}