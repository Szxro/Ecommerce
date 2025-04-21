using FluentValidation;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.AddShoppingCartCommand;

public class AddCartProductCommandValidator : AbstractValidator<AddCartProductCommand>
{
    public AddCartProductCommandValidator()
    {
        RuleFor(x => x.productName)
            .NotEmpty().WithMessage("The product name can't be empty")
            .NotNull().WithMessage("The product name can't be null");

        RuleFor(x => x.quantity)
            .GreaterThan(0).WithMessage("The product quantity must be greater than 0");
    }
}
