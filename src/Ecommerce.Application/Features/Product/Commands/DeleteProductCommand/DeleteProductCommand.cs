using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Errors;

namespace Ecommerce.Application.Features.Products.Commands.DeleteProductCommand;

public record DeleteProductCommand(string productName) : ICommand;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.productName) || string.IsNullOrWhiteSpace(request.productName))
        {
            return Result.Failure(Error.Validation("The product name must be provided"));
        }

        Product? foundProduct = await _productRepository.GetProductByNameAsync(request.productName, cancellationToken);

        if (foundProduct is null)
        {
            return Result.Failure(ProductErrors.ProductNotFoundByName(request.productName));
        }

        _productRepository.Remove(foundProduct);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
