using Ecommerce.Application.Common.DTOs.Response.Queries;
using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.Common.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Products.Queries.GetProductQuery;

public record GetProductsQuery(int page,
                               int pageSize,
                               string? searchTerm = null,
                               string? sortColumn = null,
                               string? sortOrder = null) : IQuery<OffSetPagination<ProductResponse>>;

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, OffSetPagination<ProductResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetProductsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<OffSetPagination<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Product> productQuery = _dbContext.Product;

        if (!string.IsNullOrEmpty(request.searchTerm) || !string.IsNullOrWhiteSpace(request.searchTerm))
        {
            productQuery = productQuery
                                .Where(x => x.Name.ToLower().Contains(request.searchTerm.ToLower()));
        }

        if (request.sortOrder?.ToLower() == "desc")
        {
            productQuery = productQuery.OrderByDescending(GetSortProperty(request));
        }
        else
        {
            productQuery = productQuery.OrderBy(GetSortProperty(request));
        }

        IQueryable<ProductResponse> responseQuery = productQuery
            .Include(x => x.ProductCategory)
            .Select(x => new ProductResponse
            {
                ProductName = x.Name,
                ProductDescription = x.Description,
                ProductPrice = x.Price,
                ProductCategory = x.ProductCategory.Name
            });

        OffSetPagination<ProductResponse> products = await OffSetPagination<ProductResponse>.CreateAsync(
            responseQuery,
            request.page,
            request.pageSize);

        return Result<OffSetPagination<ProductResponse>>.Success(products);
    }

    private static Expression<Func<Product, object>> GetSortProperty(GetProductsQuery productsQuery)
        => productsQuery.sortColumn?.ToLower() switch
        {
            "name" => product => product.Name,
            "description" => product => product.Description,
            "price" => product => product.Price,
            "category" => product => product.ProductCategory.Name,
            _ => product => product.Name
        };
}