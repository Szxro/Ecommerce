using Ecommerce.Application.Common.DTOs.Response;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Events;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Category = Ecommerce.SharedKernel.Enums.TemplateCategory;

namespace Ecommerce.Application.Features.Users.Commands.LoginUserCommand;

public record LoginUserCommand(string username,string password) : ICommand<TokenResponse>;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, TokenResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private static readonly TimeSpan LockOut = TimeSpan.FromMinutes(5);

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IHashService hashService,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task<Result<TokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? foundUser = await _userRepository.GetUserByUsernameAsync(request.username, cancellationToken);

        if (foundUser is null)
        {
            return Result<TokenResponse>.Failure(UserErrors.UserNotFoundByUsername); 
        }

        if (foundUser.LockOutEnabled && foundUser.LockOutEndAtUtc > DateTime.UtcNow)
        {
            return Result<TokenResponse>.Failure(UserErrors.UserLockOut(foundUser.Username.Value,foundUser.LockOutEndAtUtc));
        }

        if (foundUser.AccessFailedCount >= 3)
        {
            await HandleLockOutAsync(foundUser, cancellationToken);

            return Result<TokenResponse>.Failure(UserErrors.UserLockOut(foundUser.Username.Value, foundUser.LockOutEndAtUtc));
        }

        (string currentHash, string currentSalt) = foundUser.Credentials.Where(x => x.IsActive).Select(x => (x.HashValue, x.SaltValue)).First();

        bool isPasswordValid = _hashService.VerifyHash(request.password, currentHash, currentSalt);

        if (!isPasswordValid)
        {
            await HandleFailedLogInAttemptAsync(foundUser, cancellationToken);

            return Result<TokenResponse>.Failure(UserErrors.UserInvalidCredentials(foundUser.AccessFailedCount));
        }

        RefreshToken? unusedRefreshToken = await _refreshTokenRepository.GetUnusedUserRefreshTokenByUsernameAsync(request.username, cancellationToken);

        if (unusedRefreshToken is not null)
        {
            unusedRefreshToken.IsRevoked = true;

            _refreshTokenRepository.Update(unusedRefreshToken);
        }

        // Resetting failed count after successfully log in into the system
        foundUser.AccessFailedCount = 0;

        _userRepository.Update(foundUser);

        string refreshToken = _tokenService.GenerateRefreshToken();

        RefreshToken? newRefreshToken = new RefreshToken
        {
            Token = refreshToken,
            User = foundUser
        };

        _refreshTokenRepository.Add(newRefreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TokenResponse>.Success(
            new TokenResponse(
            _tokenService.GenerateToken(foundUser),
            refreshToken));
    }

    private async Task HandleLockOutAsync(User user, CancellationToken cancellationToken = default)
    {
        user.LockOutEndAtUtc = DateTime.UtcNow.Add(LockOut);

        user.AccessFailedCount = 0;

        // Notify user about the lock out
        user.AddEvent(new TemplateRenderEvent(
            Category.Lockout,
            new { Username = user.Username.Value },
            user.Email));

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task HandleFailedLogInAttemptAsync(User user, CancellationToken cancellationToken = default)
    {
        user.AccessFailedCount += 1;

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
