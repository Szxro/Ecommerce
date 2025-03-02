using Ecommerce.Application.Features.RefreshTokens.Commands;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.Application.Common.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.WebApi.Extensions;
using Ecommerce.WebApi.Common;

namespace Ecommerce.WebApi.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ISender _sender;

    public TokenController(ISender sender)
    {
        _sender = sender;
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("regenerate-token")]
    public async Task<IResult> RegenerateToken(RegenerateTokenCommand regenerateTokenCommand)
    {
        Result<TokenResponse> result = await _sender.Send(regenerateTokenCommand);

        return result.Match(
            onSuccess: () => CustomResult.Success(result),
            onFailure: CustomResult.Problem);
    }
}
