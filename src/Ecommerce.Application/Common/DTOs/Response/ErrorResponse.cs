namespace Ecommerce.Application.Common.DTOs.Response;

public class ErrorResponse
{
    public ErrorResponse(string errorCode, string description)
    {
        ErrorCode = errorCode;
        Description = description;
    }

    public string ErrorCode { get; }

    public string Description { get; }
}
