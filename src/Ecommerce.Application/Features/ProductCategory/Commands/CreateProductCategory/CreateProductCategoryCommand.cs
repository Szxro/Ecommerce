using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;

namespace Ecommerce.Application.Features.ProductCategories.Commands.CreateProductCategory;

public record CreateProductCategoryCommand(string name):ICommand;

public class CreateProductCategoryCommandHandler : ICommandHandler<CreateProductCategoryCommand>
{
    private readonly IProductCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public CreateProductCategoryCommandHandler(
        IProductCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }
    public async Task<Result> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.name) || string.IsNullOrWhiteSpace(request.name))
        {
            return Result.Failure(Error.Validation("The product category can't be null or empty or have white spaces"));
        }

        if (await _categoryRepository.IsProductCategoryNameNotUnique(request.name,cancellationToken))
        {
            return Result.Failure(ProductCategoryErrors.ProductCategoryAlreadyRegistered);
        }

        ProductCategory newCategory = new ProductCategory { Name = request.name };

        _categoryRepository.Add(newCategory);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _cacheService.Remove("get-all-products-categories");

        return Result.Success();
    }
}
