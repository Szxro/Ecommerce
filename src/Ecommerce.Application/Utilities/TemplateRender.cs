using Ecommerce.Application.Common.Exceptions;
using Fluid;

namespace Ecommerce.Application.Utilities;

public class TemplateRender
{
    private static readonly FluidParser _parser = new FluidParser();

    public static string Render(string template,object model)
    {
        if (_parser.TryParse(template, out IFluidTemplate fluidTemplate, out string error))
        {
            TemplateContext context = new TemplateContext(model);

            string render = fluidTemplate.Render(context);

            return render;
        }

        throw new TemplateRenderException(error);
    }
}
