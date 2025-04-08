using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class ProductCategoryErrors
{
    public static Error ProductCategoryNotFound(string categoryName) 
        => Error.NotFound($"The category '{categoryName}' was not found.");

    public static Error ProductCategoryAlreadyRegistered
        => Error.Conflict("The product category is already registered.");
}
