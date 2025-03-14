using Ecommerce.Application.Common.DTOs.Response;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Enums;
using FluentValidation.Results;

namespace Ecommerce.Application.Common.Errors;

public class ValidationError : Error
{
    public Dictionary<string, List<ErrorResponse>> Errors { get; }

    public ValidationError(ValidationFailure[] failures)
     : base("Validation.Error",
            "One or more validation errors occurred.",
            ErrorType.Validation)
    {
        Errors = GroupFailuresByPropertyName(failures);
    }

    private Dictionary<string, List<ErrorResponse>> GroupFailuresByPropertyName(ValidationFailure[] failures)
    {
        return failures
            .GroupBy(property => property.PropertyName)
            .ToDictionary(
                property => property.Key,
                property => 
                    property.Select(x => new ErrorResponse(x.ErrorCode, x.ErrorMessage)).ToList());
    }
}