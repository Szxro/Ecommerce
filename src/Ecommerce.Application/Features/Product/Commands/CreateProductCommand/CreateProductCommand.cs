using Ecommerce.Application.Common.DTOs.Entities;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;

namespace Ecommerce.Application.Features.Products.Commands.CreateProductCommand;

public record CreateProductCommand(string name, string description, string productCategory) : ICommand<ProductDTO>;


public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<ProductDTO>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await _productRepository.IsProductNameNotUnique(request.name, cancellationToken))
        {
            return Result<ProductDTO>.Failure(ProductErrors.ProductNameAlreadyRegistered);
        }
        
        ProductCategory? foundCategory = await _productCategoryRepository.GetProductCategoryByNameAsync(request.productCategory, cancellationToken);

        if (foundCategory is null)
        {
            return Result<ProductDTO>.Failure(ProductCategoryErrors.ProductCategoryNotFound(request.productCategory));
        }

        Product newProduct = new Product
        {
            Name = request.name,
            Description = request.description,
            ProductCategory = foundCategory
        };

        _unitOfWork.ChangeTrackerToUnchanged(newProduct.ProductCategory);

        _productRepository.Add(newProduct);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProductDTO>.Success(
            new ProductDTO(newProduct.Name,newProduct.Description,newProduct.ProductCategory.Name));        
    }
}