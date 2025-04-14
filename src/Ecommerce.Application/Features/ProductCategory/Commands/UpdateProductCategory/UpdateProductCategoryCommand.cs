using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;

namespace Ecommerce.Application.Features.ProductCategories.Commands.UpdateProductCategory;

public record UpdateProductCategoryCommand(string oldCategoryName,string? newCategoryName = null): ICommand;

public class UpdateProductCategoryCommandHandler : ICommandHandler<UpdateProductCategoryCommand>
{
    private readonly IProductCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateProductCategoryCommandHandler(
        IProductCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }
    public async Task<Result> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.oldCategoryName) || string.IsNullOrWhiteSpace(request.oldCategoryName))
        {
            return Result.Failure(Error.Validation("The old category name can't be null or with whitespaces"));
        }

        bool hasModified = false;

        ProductCategory? foundCategory = await _categoryRepository.GetProductCategoryByNameAsync(request.oldCategoryName);

        if (foundCategory is null)
        {
            return Result.Failure(ProductCategoryErrors.ProductCategoryNotFound(request.oldCategoryName));
        }

        if (!string.IsNullOrEmpty(request.newCategoryName) && !string.IsNullOrWhiteSpace(request.newCategoryName))
        {
            if (await _categoryRepository.IsProductCategoryNameNotUnique(request.newCategoryName) && request.newCategoryName != request.oldCategoryName)
            {
                return Result.Failure(ProductCategoryErrors.ProductCategoryAlreadyRegistered);
            }

            foundCategory.Name = request.newCategoryName;

            hasModified = true;
        }

        if (hasModified)
        {
            _categoryRepository.Update(foundCategory);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _cacheService.Remove("get-all-products-categories");
        }

        return Result.Success();
    }
}