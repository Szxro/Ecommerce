using Ecommerce.Application.Common.Data;
using Ecommerce.Application.Common.DTOs.Response.Queries;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Errors;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.ShoppingCarts.Queries.GetCurrentCartQuery;

public record GetCurrentCartQuery : IQuery<ShoppingCartDTO>;

public class GetCurrentCartQueryHandler : IQueryHandler<GetCurrentCartQuery,ShoppingCartDTO>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetCurrentCartQueryHandler(
        IApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService)
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ShoppingCartDTO>> Handle(GetCurrentCartQuery request, CancellationToken cancellationToken)
    {
        string? currentUsername = _currentUserService.GetCurrentUserName();

        if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrWhiteSpace(currentUsername))
        {
            return Result<ShoppingCartDTO>.Failure(UserErrors.UsernameNotUniqueOrInvalid);
        }

        ShoppingCartDTO? currentCart = await _applicationDbContext.ShoppingCart
                                                     .AsNoTracking()
                                                     .Include(x => x.ShoppingCartDetails)
                                                     .ThenInclude(x => x.Product)
                                                     .Where(x => x.User.Username.Value == currentUsername && !x.IsSubmit && x.TotalPrice > 0)
                                                     .Select(x => new ShoppingCartDTO
                                                     {
                                                         Details = x.ShoppingCartDetails
                                                                    .Where(x => !x.IsRemoved && x.Quantity > 0)
                                                                    .Select(x => new ShoppingCartDetailsDTO
                                                                    {
                                                                        ProductName = x.Product.Name,
                                                                        ProductPrice = x.Product.Price,
                                                                        Quantity = x.Quantity
                                                                    })
                                                                    .ToList(),
                                                         TotalPrice = x.TotalPrice
                                                     })
                                                     .FirstOrDefaultAsync(cancellationToken);

        if (currentCart is null)
        {
            return Result<ShoppingCartDTO>.Failure(ShoppingCartErrors.ShoppingCartNotFoundByUsername);
        }

        return Result<ShoppingCartDTO>.Success(currentCart);
    }
}
