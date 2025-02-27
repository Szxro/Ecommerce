using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class EmailCodeErrors
{
    public static Error EmailCodeNotFound(string code)
        => Error.NotFound($"The email code '{code}' was not found.");

    public static Error EmailCodeAlreadyUsed(string code)
        => Error.Validation($"The email code '{code}' is already used.");

    public static Error EmailCodeAlreadyRevoked(string code)
        => Error.Validation($"The email code '{code}' is already revoked.");

    public static Error EmailCodeAlreadyExpired(string code)
        => Error.Validation($"The email code '{code}' is already expired.");

    public static Error UserAlreadyVerified(string username)
        => Error.Validation($"The current user with the username '{username}' is already verified");
}