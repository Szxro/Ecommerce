using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.AddShoppingCartCommand;

public record AddCartProductCommand(string productName, int quantity) : ICommand;

public class AddCartProductCommandHandler : ICommandHandler<AddCartProductCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ICurrentUserService _currentUserService;

    public AddCartProductCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IShoppingCartRepository shoppingCartRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _shoppingCartRepository = shoppingCartRepository;
        _currentUserService = currentUserService;
    }
    public async Task<Result> Handle(AddCartProductCommand request, CancellationToken cancellationToken)
    {
        string? currentUsername = _currentUserService.GetCurrentUserName();

        if (string.IsNullOrEmpty(currentUsername) && string.IsNullOrWhiteSpace(currentUsername)) return Result.Failure(UserErrors.UsernameNotUniqueOrInvalid);

        User? currentUser = await _userRepository.GetUserByUsernameAsync(currentUsername, cancellationToken);

        if (currentUser is null) return Result.Failure(UserErrors.UserNotFoundByUsername);

        Product? foundProduct = await _productRepository.GetProductByNameAsync(request.productName, cancellationToken);

        if (foundProduct is null) return Result.Failure(ProductErrors.ProductNotFoundByName(request.productName));

        ShoppingCart? activeShoppingCart = await _shoppingCartRepository.GetActiveShoppingCartsByUsernameAsync(currentUsername, cancellationToken);

        if (activeShoppingCart is not null)
        {            
            ShoppingCartDetails? existingCartProduct = activeShoppingCart
                .ShoppingCartDetails
                .FirstOrDefault(x => x.ProductId == foundProduct.Id);            

            if (existingCartProduct is not null)
            {              
                existingCartProduct.IsRemoved = false;
                existingCartProduct.Quantity += request.quantity;
            }
            else
            {
                ShoppingCartDetails newProductCart = new ShoppingCartDetails
                {
                    Quantity = request.quantity,
                    Product = foundProduct,
                    ShoppingCart = activeShoppingCart
                };

                activeShoppingCart.ShoppingCartDetails.Add(newProductCart);

                _unitOfWork.ChangeTrackerToUnchanged(newProductCart.Product);
            }

            activeShoppingCart.TotalPrice += request.quantity * foundProduct.Price;

            _shoppingCartRepository.Update(activeShoppingCart);
        }
        else
        {
            _unitOfWork.ChangeTrackerToUnchanged(foundProduct);

            ShoppingCart shoppingCart = new ShoppingCart
            {
                User = currentUser,
                IsSubmit = false,
                TotalPrice = request.quantity * foundProduct.Price,                       
            };

            shoppingCart.ShoppingCartDetails.Add(new ShoppingCartDetails
            {
                Product = foundProduct,
                Quantity = request.quantity
            });                        

            _unitOfWork.ChangeTrackerToUnchanged(shoppingCart.User);

            _shoppingCartRepository.Add(shoppingCart);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
