using FluentValidation;

namespace Ecommerce.Infrastructure.Options.Jwt.Validations;

public class JwtOptionsValidations : AbstractValidator<JwtOptions>
{
    public JwtOptionsValidations()
    {
        RuleFor(x => x.ValidateAudience)
           .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
           .NotNull().WithMessage("The {PropertyName} can't be null.");

        RuleFor(x => x.ValidateIssuer)
            .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
            .NotNull().WithMessage("The {PropertyName} can't be null.");

        RuleFor(x => x.ValidateLifetime)
            .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
            .NotNull().WithMessage("The {PropertyName} can't be null.");

        RuleFor(x => x.ValidAudience)
            .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
            .NotNull().WithMessage("The {PropertyName} can't be null.")
            .MinimumLength(5).WithMessage("The {PropertyName} must contain at least five characters.");

        RuleFor(x => x.ValidIssuer)
            .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
            .NotNull().WithMessage("The {PropertyName} can't be null.")
            .MinimumLength(5).WithMessage("The {PropertyName} must contain at least five characters.");

        RuleFor(x => x.SecretKey)
           .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
           .NotNull().WithMessage("The {PropertyName} can't be null.")
           .MinimumLength(32).WithMessage("The {PropertyName} must contain at least thirty two characters long.");
    }
}
