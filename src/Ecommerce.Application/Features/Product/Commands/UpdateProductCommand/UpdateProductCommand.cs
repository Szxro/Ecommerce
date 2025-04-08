using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProductCommand;

public record UpdateProductCommand(string oldName, string? newName = null, string? newDescription = null, string? newProductCategory = null) : ICommand;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _productCategoryRepository = productCategoryRepository;
    }
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.oldName) || string.IsNullOrWhiteSpace(request.oldName))
        {
            return Result.Failure(Error.Validation("The old of the product can'be null or empty."));
        }

        Product? foundProduct = await _productRepository.GetProductByNameAsync(request.oldName, cancellationToken);

        if (foundProduct is null)
        {
            return Result.Failure(ProductErrors.ProductNotFoundByName(request.oldName));
        }

        bool isModified = false;

        if (!string.IsNullOrEmpty(request.newName) && request.newName != foundProduct.Name)        
        {
            if (await _productRepository.IsProductNameNotUnique(request.newName))
            {
                return Result.Failure(ProductErrors.ProductNameAlreadyRegistered);
            }

            foundProduct.Name = request.newName;

            isModified = true;
        }

        if (!string.IsNullOrWhiteSpace(request.newDescription) && request.newDescription != foundProduct.Description)
        {
            foundProduct.Description = request.newDescription;

            isModified = true;
        }

        if (!string.IsNullOrWhiteSpace(request.newProductCategory))
        {
            ProductCategory? foundCategory = await _productCategoryRepository.GetProductCategoryByNameAsync(request.newProductCategory, cancellationToken);

            if (foundCategory is null)
            {
                return Result.Failure(ProductCategoryErrors.ProductCategoryNotFound(request.newProductCategory));
            }

            foundProduct.ProductCategory = foundCategory;

            _unitOfWork.ChangeTrackerToUnchanged(foundProduct.ProductCategory);

            isModified = true;
        }

        if (isModified)
        {
            _productRepository.Update(foundProduct);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
