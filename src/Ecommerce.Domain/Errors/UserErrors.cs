using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class UserErrors
{
    public static Error EmailNotUniqueOrInvalid
            => Error.Validation("Email is not valid or is already registered.");

    public static Error UsernameNotUniqueOrInvalid
            => Error.Validation("Username is not valid or is already registered.");
}
