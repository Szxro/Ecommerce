using FluentValidation;

namespace Ecommerce.Application.Features.Users.Commands.UploadImageCommand;

public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
    private static readonly long MaxImageSize = 2 * 1024 * 1024; // 2MB in bytes (2 * 1,048,576 = 2,097,152);

    private static readonly string[] AcceptedImageExtensions = [".jpeg", ".png", ".jpg"];

    public UploadImageCommandValidator()
    {
        RuleFor(x => x.file)
            .NotEmpty().WithMessage("The uploaded file cant be empty")
            .NotNull().WithMessage("The uploaded file cant be null")
            .When(x => x.file is not null)
            .Must(file => IsFileValid(file.Length,Path.GetExtension(file.FileName)))
            .WithMessage("The file size or extension is not allowed. the file size must be a least less than 2MB");        
    }

    private bool IsFileValid(long fileLength, string extension)
    {
        return fileLength <= MaxImageSize && AcceptedImageExtensions.Contains(extension);
    }
}
