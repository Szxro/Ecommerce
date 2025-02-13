using FluentValidation;

namespace Ecommerce.Infrastructure.Options.Hash.Validations;

public class HashOptionsValidations : AbstractValidator<HashOptions>
{
    public HashOptionsValidations()
    {
        RuleFor(x => x.Iterations)
           .NotEmpty().WithMessage("The {PropertyName} can't empty")
           .NotNull().WithMessage("The {PropertyName} can't null")
           .GreaterThan(10000).WithMessage("The {PropertyName} must be greater than 10000");

        RuleFor(x => x.SaltSize)
            .NotEmpty().WithMessage("The {PropertyName} can't empty")
            .NotNull().WithMessage("The {PropertyName} can't null")
            .GreaterThan(0).WithMessage("The {PropertyName} must be greater than 0");

        RuleFor(x => x.HashSize)
            .NotEmpty().WithMessage("The {PropertyName} can't empty")
            .NotNull().WithMessage("The {PropertyName} can't null")
            .GreaterThan(0).WithMessage("The {PropertyName} must be greater than 0");
    }
}
