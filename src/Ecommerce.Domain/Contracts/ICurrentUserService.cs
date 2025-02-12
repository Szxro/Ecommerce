namespace Ecommerce.Domain.Contracts;

public interface ICurrentUserService
{
    string? GetCurrentUserName();
}
