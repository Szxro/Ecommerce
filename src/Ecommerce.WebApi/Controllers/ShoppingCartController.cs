using Ecommerce.Application.Features.ShoppingCarts.AddShoppingCartCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.WebApi.Extensions;
using Ecommerce.WebApi.Common;

namespace Ecommerce.WebApi.Controllers;

[Route("api/cart")]
[Authorize]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly ISender _sender;

    public ShoppingCartController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IResult> AddToCart(AddShoppingCartCommand addShoppingCart)
    {
        Result result = await _sender.Send(addShoppingCart);

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }
}
