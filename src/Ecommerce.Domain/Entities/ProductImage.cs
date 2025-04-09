using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class ProductImage : IntermediaryEntity
{
    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public int ImageId { get; set; }

    public Image Image { get; set; } = null!;
}
