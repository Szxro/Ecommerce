using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Errors;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.RemoveCartProductCommand;

public record RemoveCartProductCommand(string productName, int quantity = 0, bool isRemoved = false) : ICommand;

public class RemoveCartProductCommandHandler : ICommandHandler<RemoveCartProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ICurrentUserService _currentUserService;    
    private readonly IProductRepository _productRepository;

    public RemoveCartProductCommandHandler(
        IUnitOfWork unitOfWork,
        IShoppingCartRepository shoppingCartRepository,
        ICurrentUserService currentUserService,        
        IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _shoppingCartRepository = shoppingCartRepository;
        _currentUserService = currentUserService;        
        _productRepository = productRepository;
    }
    public async Task<Result> Handle(RemoveCartProductCommand request, CancellationToken cancellationToken)
    {
        bool isModified = false;

        string? currentUsername = _currentUserService.GetCurrentUserName();

        if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrWhiteSpace(currentUsername)) return Result.Failure(UserErrors.UsernameNotUniqueOrInvalid);

        Product? product = await _productRepository.GetProductByNameAsync(request.productName);

        if (product is null) return Result.Failure(ProductErrors.ProductNotFoundByName(request.productName));

        ShoppingCart? shoppingCart = await _shoppingCartRepository.GetActiveShoppingCartsByUsernameAsync(currentUsername);

        if (shoppingCart is null)
        {
            return Result.Failure(ShoppingCartErrors.ShoppingCartNotFoundByUsername);
        }

        ShoppingCartDetails? cartDetails = 
            shoppingCart.ShoppingCartDetails.Where(x => x.ProductId == product.Id && !x.IsRemoved).FirstOrDefault();

        if (cartDetails is null)
        {
            return Result.Failure(Error.NotFound("The product was not found in the shopping cart."));
        }

        if (request.isRemoved)
        {
            cartDetails.IsRemoved = request.isRemoved;

            shoppingCart.TotalPrice -= cartDetails.Quantity * product.Price;

            cartDetails.Quantity = 0;

            isModified = true;

        } else if (request.quantity > 0 && request.quantity <= cartDetails.Quantity)
        {
            cartDetails.Quantity -= request.quantity;

            shoppingCart.TotalPrice -= request.quantity * product.Price;

            isModified = true;
        }        

        if (!isModified)
        {
            return Result.Failure(Error.Validation("No modifications were made."));
        }

        _shoppingCartRepository.Update(shoppingCart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
