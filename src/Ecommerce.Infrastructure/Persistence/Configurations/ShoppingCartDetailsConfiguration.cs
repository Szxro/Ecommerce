using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class ShoppingCartDetailsConfiguration : IEntityTypeConfiguration<ShoppingCartDetails>
{
    public void Configure(EntityTypeBuilder<ShoppingCartDetails> builder)
    {
        // Note: Creating a custom unique index (can appear just one product by shopping cart)
        builder
            .HasIndex(x => new { x.ShoppingCartId, x.ProductId })
            .IsUnique()
            .HasDatabaseName("UX_ShoppingCartDetails_ShoppingCartId_ProductId");

        builder.ToTable("shopping_cart_details");
    }
}
