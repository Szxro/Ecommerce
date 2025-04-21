using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.WebApi.Extensions;
using Ecommerce.WebApi.Common;
using Ecommerce.Application.Features.ShoppingCarts.Commands.AddShoppingCartCommand;
using Ecommerce.Application.Features.ShoppingCarts.Commands.RemoveCartProductCommand;
using Ecommerce.Application.Features.ShoppingCarts.Queries.GetCurrentCartQuery;
using Ecommerce.Application.Common.DTOs.Response.Queries;

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
    public async Task<IResult> AddToCart(AddCartProductCommand addShoppingCart)
    {
        Result result = await _sender.Send(addShoppingCart);

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }

    [HttpDelete]

    public async Task<IResult> RemoveFromCart(RemoveCartProductCommand removeCart)
    {
        Result result = await _sender.Send(removeCart);

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }

    [HttpGet]
    public async Task<IResult> GetCart()
    {
        Result<ShoppingCartDTO> result = await _sender.Send(new GetCurrentCartQuery());

        return result.Match(
            onSuccess: () => CustomResult.Success(result),
            onFailure: CustomResult.Problem);
    }
}
