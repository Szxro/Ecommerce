using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class ShoppingCartErrors
{
    public static Error ShoppingCartNotFoundByUsername
        => Error.NotFound("No Active shopping cart were found by the given username, check and try again.");
}
