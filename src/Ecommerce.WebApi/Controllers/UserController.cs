using Ecommerce.Application.Features.Users.Commands.CreateUserCommand;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.WebApi.Common;
using Ecommerce.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IResult> RegisterNewUser(CreateUserCommand userCommand)
    {
        Result result = await _sender.Send(userCommand);

        return result.Match(
            onSuccess: Results.Created,
            onFailure: CustomResult.Problem);
    }
}
