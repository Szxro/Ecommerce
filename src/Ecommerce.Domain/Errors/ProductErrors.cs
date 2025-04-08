using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.Domain.Errors;

public static class ProductErrors
{
    public static Error ProductNameAlreadyRegistered
        => Error.Conflict("Product name already registered.");
}
