using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Common.DTOs.Response.Queries;

public class ShoppingCartDetailsDTO
{
    public string ProductName { get; set; } = string.Empty;

    public int ProductPrice { get; set; }

    public int Quantity { get; set; }
}

public class ShoppingCartDTO
{
    public List<ShoppingCartDetailsDTO> Details { get; set; } = null!;

    public int TotalPrice { get; set; }
}
