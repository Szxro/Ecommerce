using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class ShoppingCartProductConfiguration : IEntityTypeConfiguration<ShoppingCartProduct>
{
    public void Configure(EntityTypeBuilder<ShoppingCartProduct> builder)
    {
        // Note: Creating a custom unique index (can appear just one product by shopping cart)
        builder
            .HasIndex(x => new { x.ShoppingCartId, x.ProductId })
            .IsUnique()
            .HasDatabaseName("UX_ShoppingCartProduct_ShoppingCartId_ProductId");
    }
}
