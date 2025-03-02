namespace Ecommerce.Application.Common.Exceptions;

public class TemplateRenderException : Exception
{
    public TemplateRenderException(string error) 
        : base($"Template parsing failed. Error: {error}")
    {
        
    }
}
