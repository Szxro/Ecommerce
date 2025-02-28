using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class RefreshTokenErrors
{
    public static Error InvalidRefreshToken
        => Error.Validation("The provided refresh token is expired or used.");
}
