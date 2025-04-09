using FluentValidation;
using Ecommerce.Application.Utilities;

namespace Ecommerce.Application.Features.Products.Commands.UploadProductImageCommand;

public class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommand>
{
    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.productName)
            .NotEmpty().WithMessage("The product name can't be empty")
            .NotNull().WithMessage("The product name can't be null");

        RuleFor(x => x.file)
            .NotEmpty().WithMessage("The uploaded file cant be empty")
            .NotNull().WithMessage("The uploaded file cant be null")
            .When(x => x.file is not null)
            .MustBeAValidFile();
    }
}
