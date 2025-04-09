using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.SharedKernel.Enums;
using Microsoft.AspNetCore.Http;
using Ecommerce.Domain.Results;
using Ecommerce.Application.Common.Data;

namespace Ecommerce.Application.Features.Products.Commands.UploadProductImageCommand;

public record UploadProductImageCommand(string productName,IFormFile file) : ICommand;

public class UploadProductImageCommandHandler : ICommandHandler<UploadProductImageCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IMediaCompressionService _mediaCompressionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationDbContext _applicationDb;

    public UploadProductImageCommandHandler(
        IProductRepository productRepository,
        IMediaCompressionService mediaCompressionService,
        IUnitOfWork unitOfWork,
        IApplicationDbContext applicationDb)
    {
        _productRepository = productRepository;
        _mediaCompressionService = mediaCompressionService;
        _unitOfWork = unitOfWork;
        _applicationDb = applicationDb;
    }
    public async Task<Result> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        Product? foundProduct = await _productRepository.GetProductByNameAsync(request.productName, cancellationToken);

        if (foundProduct is null)
        {
            return Result.Failure(ProductErrors.ProductNotFoundByName(request.productName));
        }

        using Stream stream = request.file.OpenReadStream();

        Result<CompressionResult> compressionResult = _mediaCompressionService.ImageCompressionAndSave(
            stream,
            request.file.Length,
            Path.GetExtension(request.file.FileName),
            ImageQuality.Good);

        if (!compressionResult.IsSuccess) return compressionResult;

        (string filename, string path, long totalSize, int height, int width, string format) = compressionResult.Value;

        ProductImage newProductImage = new ProductImage
        {
            Product = foundProduct,
            Image = new Image
            {
                FileName = filename,
                Format = format,
                Path = path,
                TotalSize = (int)totalSize,
                Height = height,
                Width = width,                
            }
        };

        _unitOfWork.ChangeTrackerToUnchanged(newProductImage.Product);

        _applicationDb.ProductImage.Add(newProductImage);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
