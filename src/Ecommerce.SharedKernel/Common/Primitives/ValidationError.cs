using Ecommerce.SharedKernel.Enums;

namespace Ecommerce.SharedKernel.Common.Primitives;

public class ValidationError : Error
{
    public ValidationError(Error[] errors)
      : base("Validation.Error",
             "One or more validation errors occurred.",
             ErrorType.Validation)
    {
        Errors = errors;
    }

    public Error[] Errors { get; set; }
}
