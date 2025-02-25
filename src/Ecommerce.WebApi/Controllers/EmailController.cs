using Ecommerce.Application.Features.EmailCodes.Commands.VerifyEmailCode;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.WebApi.Common;
using Ecommerce.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers;

[Route("api/email")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly ISender _sender;

    public EmailController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("verify-email-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> VerifyEmailCode([FromQuery] string emailCode)
    {
        Result result = await _sender.Send(new VerifyEmailCodeCommand(emailCode));

        return result.Match(
            onSuccess: () => Results.Ok(),
            onFailure: CustomResult.Problem);
    }
}
