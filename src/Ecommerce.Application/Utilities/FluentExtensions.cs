using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Utilities;

public static class FluentExtensions
{
    public static IRuleBuilderOptions<T,IFormFile> MustBeAValidFile<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
    {
        return ruleBuilder
            .Must(file => ApplicationUtilities.IsFileValid(file.Length,Path.GetExtension(file.FileName)))
            .WithMessage("The file size or extension is not allowed. the file size must be a least less than 2MB");
    }
}
