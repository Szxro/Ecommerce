using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.SharedKernel.Common.Primitives;
using TemplateCategory = Ecommerce.SharedKernel.Enums.TemplateCategory;
using Fluid;

namespace Ecommerce.Application.Utilities;

public static class ApplicationUtilities
{
    private static readonly FluidParser _parser = new FluidParser();

    private static readonly long MaxImageSize = 2 * 1024 * 1024; // 2MB in bytes (2 * 1,048,576 = 2,097,152);

    private static readonly string[] AcceptedImageExtensions = [".jpeg", ".png", ".jpg"];

    public static string TemplateRender(string template,object model)
    {
        if (_parser.TryParse(template, out IFluidTemplate fluidTemplate, out string error))
        {
            TemplateContext context = new TemplateContext(model);

            string render = fluidTemplate.Render(context);

            return render;
        }

        throw new TemplateRenderException(error);
    }

    public static string GetTemplateCategoryDescription(TemplateCategory category)
        => category switch
        {
            TemplateCategory.VerificationEmail => "verification_email",
            TemplateCategory.Lockout => "lockout_email",
            TemplateCategory.WelcomeMessage => "welcome_message",
            TemplateCategory.ResetPasswordEmail => "reset_password_email",
            _ => throw new ArgumentOutOfRangeException()
        };

    public static Result ValidateEmailCode(EmailCode emailCode)
       => emailCode switch
       {
           { IsUsed: true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyUsed(emailCode.Code)),
           { IsRevoked: true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyRevoked(emailCode.Code)),
           { IsExpired: true } => Result.Failure(EmailCodeErrors.EmailCodeAlreadyExpired(emailCode.Code)),
           _ => Result.Success(),
       };

    public static bool IsFileValid(long fileLength, string extension)
    {
        return fileLength <= MaxImageSize && AcceptedImageExtensions.Contains(extension);
    }
}
