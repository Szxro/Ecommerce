using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class TokenErrors
{
    public static Error InvalidToken
        => Error.Validation("The provided token is invalid.");

    public static Error InvalidTokenClaims
        => Error.Validation( "Token does not contain valid claims");

    public static Error InvalidTokenExpiryStamp
        => Error.Validation("Token expiration timestamp is invalid.");

    public static Error TokenStillValid
        => Error.Validation("The provided token is still valid.");
}
