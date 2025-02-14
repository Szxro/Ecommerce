namespace Ecommerce.SharedKernel.Common;

public class EmailMessage
{
    public string ToAddress { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;
}
