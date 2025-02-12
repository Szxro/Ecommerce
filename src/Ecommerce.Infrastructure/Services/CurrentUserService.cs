using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Ecommerce.Infrastructure.Services;

[Inject(ServiceLifetime.Scoped)]
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContext;

    public CurrentUserService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }
    public string? GetCurrentUserName()
    {
        return _httpContext?.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Name)?.Value;
    }
}
