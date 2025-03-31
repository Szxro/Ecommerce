using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Results;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.SharedKernel.Enums;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Features.Users.Commands.UploadImageCommand;

public record UploadImageCommand(IFormFile file) : ICommand;

public class UploadImageCommandHandler : ICommandHandler<UploadImageCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediaCompressionService _mediaCompressionService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserImageRepository _userImageRepository;

    public UploadImageCommandHandler(
        ICurrentUserService currentUserService,
        IMediaCompressionService mediaCompressionService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserImageRepository userImageRepository)
    {
        _currentUserService = currentUserService;
        _mediaCompressionService = mediaCompressionService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userImageRepository = userImageRepository;
    }

    public async Task<Result> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        string? username = _currentUserService.GetCurrentUserName();

        if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
        {
            return Result.Failure(UserErrors.UsernameNotUniqueOrInvalid);
        }

        User? foundUser = await _userRepository.GetUserByUsernameAsync(username, cancellationToken);

        if (foundUser is null)
        {
            return Result.Failure(UserErrors.UserNotFoundByUsername);
        }

        await HandleExistingUserImageAsync(username, cancellationToken);      

        // Opening a read stream from the upload file
        using Stream stream = request.file.OpenReadStream();

        // Compress and save the image
        Result<CompressionResult> compressionResult = _mediaCompressionService.ImageCompressionAndSave(
            stream,
            request.file.Length,
            Path.GetExtension(request.file.FileName),
            ImageQuality.Good);

        if (!compressionResult.IsSuccess) return compressionResult;

        (string filename, string path, long totalSize, int height, int width, string format) = compressionResult.Value;

        UserImage newUserImage = new UserImage
        {
            User = foundUser,
            Image = new Image
            {
                FileName = filename,
                Path = path,
                TotalSize = (int)totalSize,
                Height = height,
                Width = width,
                Format = format
            },
            IsActive = true 
        };

        _userImageRepository.Add(newUserImage);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task HandleExistingUserImageAsync(string username, CancellationToken cancellationToken = default)
    {
        UserImage? foundUserImage = await _userImageRepository.GetActiveUserImageByUsernameAsync(username, cancellationToken);

        if (foundUserImage is not null)
        {
            foundUserImage.IsActive = false;

            _userImageRepository.Update(foundUserImage);
        }
    }
}
