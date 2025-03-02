using Ecommerce.Infrastructure.Options.Smtp;
using FluentValidation;

namespace Ecommerce.Infrastructure.Options.SmtpServer.Validations;

public class SmtpServerOptionsValidations : AbstractValidator<SmtpServerOptions>
{
    public SmtpServerOptionsValidations()
    {
        RuleFor(x => x.Host)
           .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
           .NotNull().WithMessage("The {PropertyName} can't be null.")
           .Matches(@"^[\w\.-]+$").WithMessage("Host must be a valid hostname.");

        RuleFor(x => x.Port)
            .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
            .NotNull().WithMessage("The {PropertyName} can't be null.")
            .InclusiveBetween(1, 65535).WithMessage("The {PropertyName} must be between 1 and 65535.");

        RuleFor(x => x.Username)
            .NotNull().WithMessage("The {PropertyName} can't be null.")
            .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
            .Length(1, 256).WithMessage("The {PropertyName} must be between 1 and 256 characters.");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("The {PropertyName} can't be null.")
            .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
            .Length(6, 100).WithMessage("The {PropertyName} must be between 6 and 100 characters.");

        RuleFor(x => x.UseSsl)
            .NotNull().WithMessage("The {PropertyName} can't be null");

        RuleFor(x => x.FromAddress)
            .NotEmpty().WithMessage("The {PropertyName} can't be empty.")
            .NotNull().WithMessage("The {PropertyName} can't be null.")
            .EmailAddress().WithMessage("The {Property} must be a valid email");
    }
}
