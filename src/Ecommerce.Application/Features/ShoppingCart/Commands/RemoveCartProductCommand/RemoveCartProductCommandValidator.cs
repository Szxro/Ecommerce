using FluentValidation;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.RemoveCartProductCommand;

class RemoveCartProductCommandValidator : AbstractValidator<RemoveCartProductCommand>
{
    public RemoveCartProductCommandValidator()
    {
        RuleFor(x => x.productName)
            .NotEmpty().WithMessage("The product name can't be empty")
            .NotNull().WithMessage("The product name can't be null");
    }
}
