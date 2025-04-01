using Ecommerce.Application.Common.Exceptions;
using Ecommerce.SharedKernel.Enums;
using Fluid;

namespace Ecommerce.Application.Utilities;

public static class ApplicationUtilities
{
    private static readonly FluidParser _parser = new FluidParser();

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
}
