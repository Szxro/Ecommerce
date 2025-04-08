using FluentValidation;

namespace Ecommerce.Application.Features.Products.Commands.CreateProductCommand;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.name)
            .NotNull().WithMessage("The product name can't be null")
            .NotEmpty().WithMessage("The product name can't be empty");

        RuleFor(x => x.description)
            .NotNull().WithMessage("The product description can't be null")
            .NotEmpty().WithMessage("The product description can't be empty");

        RuleFor(x => x.productCategory)
            .NotNull().WithMessage("The product category can't be null")
            .NotEmpty().WithMessage("The product category can't be empty");
    }
}
