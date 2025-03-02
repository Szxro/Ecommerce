using Ecommerce.Application.Features.Users.Commands.RegisterUserCommand;
using Ecommerce.Application.Features.Users.Commands.LoginUserCommand;
using Ecommerce.Application.Common.DTOs.Response;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.WebApi.Common;
using Ecommerce.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Ecommerce.WebApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register-user")]
    public async Task<IResult> RegisterNewUser(RegisterUserCommand userCommand)
    {
        Result result = await _sender.Send(userCommand);

        return result.Match(
            onSuccess: Results.Created,
            onFailure: CustomResult.Problem);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("login-user")]
    public async Task<IResult> LoginUser(LoginUserCommand loginUserCommand)
    {
        Result<TokenResponse> result = await _sender.Send(loginUserCommand);

        return result.Match(
            onSuccess: () => CustomResult.Success(result),
            onFailure: CustomResult.Problem);
    }
}
