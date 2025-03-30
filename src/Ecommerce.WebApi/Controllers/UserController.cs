using Ecommerce.Application.Features.Users.Commands.RegisterUserCommand;
using Ecommerce.Application.Features.Users.Commands.LoginUserCommand;
using Ecommerce.Application.Features.EmailCodes.Commands.ResendEmailCode;
using Ecommerce.Application.Features.EmailCodes.Commands.VerifyEmailCode;
using Ecommerce.Application.Features.Users.Commands.UploadImageCommand;
using Ecommerce.Application.Features.Users.Commands.CreateAddressCommand;
using Ecommerce.Application.Common.DTOs.Response;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.WebApi.Common;
using Ecommerce.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebApi.Controllers;

[Route("api/users")]
[Authorize]
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
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IResult> RegisterUser(RegisterUserCommand userCommand)
    {
        Result result = await _sender.Send(userCommand);

        return result.Match(
            onSuccess: Results.Created,
            onFailure: CustomResult.Problem);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IResult> LoginUser(LoginUserCommand loginUserCommand)
    {
        Result<TokenResponse> result = await _sender.Send(loginUserCommand);

        return result.Match(
            onSuccess: () => CustomResult.Success(result),
            onFailure: CustomResult.Problem);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [HttpGet("verify-email")]
    public async Task<IResult> VerifyEmailCode([FromQuery] string emailCode)
    {
        Result result = await _sender.Send(new VerifyEmailCodeCommand(emailCode));

        return result.Match(
            onSuccess: () => Results.Ok(),
            onFailure: CustomResult.Problem);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    [HttpPost("{username}/resend-email")]
    public async Task<IResult> ResendEmailCode([FromRoute] string username)
    {
        Result result = await _sender.Send(new ResendEmailCodeCommand(username));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }

    [HttpPost("upload-image")]
    public async Task<IResult> UploadImage(IFormFile formFile)
    {
        Result result = await _sender.Send(new UploadImageCommand(formFile));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }

    [HttpPost("addresses")]
    public async Task<IResult> CreateUserAddress(CreateAddressCommand addressCommand)
    {
        Result result = await _sender.Send(addressCommand);

        return result.Match(
            onSuccess: Results.Created,
            onFailure: CustomResult.Problem);
    }
}
