using Ecommerce.Application.Common.DTOs.Response;
using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using System.Security.Claims;
using Ecommerce.Domain.Errors;

namespace Ecommerce.Application.Features.RefreshTokens.Commands;

public record RegenerateTokenCommand(string token,string refreshToken) : ICommand<TokenResponse> { }

public class RegenerateTokenCommandHandler : ICommandHandler<RegenerateTokenCommand, TokenResponse>
{
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    private const string TokenUsernameType = "name";
    private const string TokenExpirateStampType = "exp";

    public RegenerateTokenCommandHandler(
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<TokenResponse>> Handle(RegenerateTokenCommand request, CancellationToken cancellationToken)
    {
        (bool isValid, ClaimsIdentity? claims) = await _tokenService.ValidateToken(request.token);

        if (!isValid && claims is null)
        {
            return Result<TokenResponse>.Failure(TokenErrors.InvalidToken);
        }

        (string? username, string? expiryDateStamp) = ExtractClaims(claims);

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(expiryDateStamp))
        {
            return Result<TokenResponse>.Failure(TokenErrors.InvalidTokenClaims);
        }

        if (!long.TryParse(expiryDateStamp,out long expiryTimestamp))
        {
            return Result<TokenResponse>.Failure(TokenErrors.InvalidTokenExpiryStamp);
        }

        DateTime tokenExpiryDate = TimeStampToUTCDate(expiryTimestamp);

        if (DateTime.UtcNow < tokenExpiryDate)
        {
            return Result<TokenResponse>.Failure(TokenErrors.TokenStillValid);
        }

        RefreshToken? validRefreshToken = await _refreshTokenRepository.GetValidRefreshTokenAsync(request.refreshToken,username, cancellationToken);

        if (validRefreshToken is null)
        {
            return Result<TokenResponse>.Failure(RefreshTokenErrors.InvalidRefreshToken);
        }

        validRefreshToken.IsUsed = true;

        _refreshTokenRepository.Update(validRefreshToken);

        string refreshToken = _tokenService.GenerateRefreshToken();

        RefreshToken newRefreshToken = new RefreshToken 
        {
            Token = refreshToken,
            User = validRefreshToken.User
        };

        _refreshTokenRepository.Add(newRefreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TokenResponse>.Success(
            new TokenResponse(
                _tokenService.GenerateToken(validRefreshToken.User),
                refreshToken)
            );
    }

    private (string? username, string? expiryDateStamp) ExtractClaims(ClaimsIdentity? claimsIdentity)
    {
        string? expiryDateStamp = claimsIdentity?.Claims.Where(x => x.Type == TokenExpirateStampType).FirstOrDefault()?.Value;

        string? username = claimsIdentity?.Claims.Where(x => x.Type == TokenUsernameType).FirstOrDefault()?.Value;

        return (username, expiryDateStamp);
    }

    private DateTime TimeStampToUTCDate(long timeStamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timeStamp).UtcDateTime;
    }
}
