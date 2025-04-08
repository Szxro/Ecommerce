using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class ProductErrors
{
    public static Error ProductNameAlreadyRegistered
        => Error.Conflict("Product name already registered.");

    public static Error ProductNotFoundByName(string productName)
        => Error.NotFound($"The product with the name '{productName}' was not found.");
}
