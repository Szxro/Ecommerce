using Ecommerce.Application.Common.Data;
using Ecommerce.Application.Common.DTOs.Response.Queries;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.ProductCategories.Queries.GetProductCategoriesQuery;

public record GetProductCategoriesQuery : ICachedQuery<List<ProductCategoryResponse>>
{
    public string CachedKey => "get-all-products-categories";
}

public class GetProductCategoryQueryHandler : IQueryHandler<GetProductCategoriesQuery, List<ProductCategoryResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetProductCategoryQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<ProductCategoryResponse>>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<ProductCategoryResponse> categories = await _dbContext
                                                                .ProductCategory
                                                                .AsNoTracking()
                                                                .Select(x => new ProductCategoryResponse(x.Name))
                                                                .ToListAsync(cancellationToken);

        return Result<List<ProductCategoryResponse>>.Success(categories);
    }
}
