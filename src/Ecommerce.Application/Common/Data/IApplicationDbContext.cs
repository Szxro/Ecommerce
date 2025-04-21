using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Common.Data;

public interface IApplicationDbContext
{
    public DbSet<Product> Product { get; }

    public DbSet<ProductImage> ProductImage { get; }

    public DbSet<ProductCategory> ProductCategory { get; }

    public DbSet<ShoppingCart> ShoppingCart { get; }
}
