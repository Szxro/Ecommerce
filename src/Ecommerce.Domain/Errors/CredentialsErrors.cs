using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class CredentialsErrors
{
    public static Error CredentialsNotFoundByUsername(string username)
        => Error.NotFound($"Credentials not found for the username '{username}'.");
}
