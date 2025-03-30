using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Errors;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Users.Commands.CreateAddressCommand;

public record CreateAddressCommand(int streetNumber,
                                   string addressLine1,
                                   string city,
                                   string region,
                                   string postalCode,
                                   string countryName,
                                   bool isDefault,
                                   string? addressLine2 = null) : ICommand;

public class CreateAddressCommandHandler : ICommandHandler<CreateAddressCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICountryRepository _countryRepository;
    private readonly IUserAddressRepository _userAddressRepository;

    public CreateAddressCommandHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICountryRepository countryRepository,
        IUserAddressRepository userAddressRepository)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _countryRepository = countryRepository;
        _userAddressRepository = userAddressRepository;
    }
    public async Task<Result> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        string? currentUserName = _currentUserService.GetCurrentUserName();

        if (string.IsNullOrEmpty(currentUserName) || string.IsNullOrWhiteSpace(currentUserName))
        {
            return Result.Failure(UserErrors.UsernameNotUniqueOrInvalid);
        }

        User? foundUser = await _userRepository.GetUserByUsernameAsync(currentUserName, cancellationToken);

        if (foundUser is null)
        {
            return Result.Failure(UserErrors.UserNotFoundByUsername);
        }

        Country? foundCountry = await _countryRepository.GetCountryByCountryNameAsync(request.countryName, cancellationToken);

        if (foundCountry is null)
        {
            return Result.Failure(Error.NotFound($"The given country with the country name of {request.countryName} was not found."));
        }

        UserAddress newUserAddress = new UserAddress
        {
            User = foundUser,
            Address = new Address
            { 
                Country = foundCountry,
                AddressLine1 = request.addressLine1,
                City = request.city,
                PostalCode = request.postalCode,
                StreetNumber = request.streetNumber,
                AddressLine2 = request.addressLine2,
                Region = request.region,                
            },
            IsDefault = request.isDefault
        };

        _unitOfWork.ChangeTrackerToUnchanged(newUserAddress.User);

        _unitOfWork.ChangeTrackerToUnchanged(newUserAddress.Address.Country);

        _userAddressRepository.Add(newUserAddress);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
