using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;

namespace Ecommerce.Application.Features.ProductCategories.Commands.DeleteProductCategoryCommand;

public record DeleteProductCategoryCommand(string categoryName): ICommand { };

public class DeleteProductCategoryCommandHandler : ICommandHandler<DeleteProductCategoryCommand>
{
    private readonly IProductCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteProductCategoryCommandHandler(
        IProductCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        ProductCategory? foundCategory = await _categoryRepository.GetProductCategoryByNameAsync(request.categoryName);

        if (foundCategory is null)
        {
            return Result.Failure(ProductCategoryErrors.ProductCategoryNotFound(request.categoryName));
        }

        _categoryRepository.Remove(foundCategory);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _cacheService.Remove("get-all-products-categories");

        return Result.Success();
    }
}
