using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace Ecommerce.WebApi.Filters;

public class IsAdminAttribute : ActionFilterAttribute
{
    // NOTE: If i want to use dependency injection a need to register the filter in the DI container;
 
    private static readonly string JwtClaimIsAdministrator = "is_admin";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!isUserAdmin(context.HttpContext))
        {
            // 403 Forbidden
            context.Result = new ForbidResult();

            context.HttpContext.Response.Headers.Append(HeaderNames.WWWAuthenticate,"is_not_admin");

            return;
        }
        
        base.OnActionExecuting(context);
    }

    private bool isUserAdmin(HttpContext context)
    {
        return context
            .User.Claims.Any(claim => claim.Type == JwtClaimIsAdministrator && claim.Value == "true");
    }
}
