namespace Ecommerce.Application.Common.DTOs.Response.Queries;

public class ProductResponse
{
    public string ProductName { get; set; } = string.Empty;

    public string ProductDescription { get; set; } = string.Empty;

    public int ProductPrice { get; set; }

    public string ProductCategory { get; set; } = string.Empty;
}
