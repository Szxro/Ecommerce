using Ecommerce.Application.Utilities;
using FluentValidation;

namespace Ecommerce.Application.Features.Users.Commands.UploadImageCommand;

public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{   
    public UploadImageCommandValidator()
    {
        RuleFor(x => x.file)
            .NotEmpty().WithMessage("The uploaded file cant be empty")
            .NotNull().WithMessage("The uploaded file cant be null")
            .When(x => x.file is not null)
            .MustBeAValidFile();
    }
}
