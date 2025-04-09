using Ecommerce.Application.Features.Products.Commands.CreateProductCommand;
using Ecommerce.WebApi.Filters;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.Application.Common.DTOs.Entities;
using Ecommerce.WebApi.Extensions;
using Ecommerce.WebApi.Common;
using Ecommerce.Application.Features.Products.Commands.UpdateProductCommand;
using Ecommerce.Application.Features.Products.Commands.DeleteProductCommand;
using Ecommerce.Application.Features.Products.Queries.GetProductQuery;
using Ecommerce.Application.Common.Data;
using Ecommerce.Application.Common.DTOs.Response.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Features.Products.Commands.UploadProductImageCommand;

namespace Ecommerce.WebApi.Controllers;

[Route("api/products")]
[Authorize]
[IsAdminFilter]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IResult> CreateProduct(CreateProductCommand createProduct)
    {
        Result<ProductDTO> result = await _sender.Send(createProduct);

        return result.Match(
            onSuccess: () => CustomResult.Success(result),
            onFailure: CustomResult.Problem);
    }

    [HttpPut]
    public async Task<IResult> UpdateProduct(UpdateProductCommand updateProduct)
    {
        Result result = await _sender.Send(updateProduct);

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }

    [HttpDelete("{productName}")]
    public async Task<IResult> DeleteProduct(string productName)
    {
        Result result = await _sender.Send(new DeleteProductCommand(productName));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> GetProducts(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int page,
        int pageSize)
    {
        Result<OffSetPagination<ProductResponse>> result = await _sender.Send(new GetProductsQuery(page, pageSize, searchTerm, sortColumn, sortOrder));

        return CustomResult.Success(result);
    }

    [HttpPost("{productName}")]
    public async Task<IResult> UploadProductImage(string productName,IFormFile formFile)
    {
        Result result = await _sender.Send(new UploadProductImageCommand(productName,formFile));

        return result.Match(
            onSuccess: Results.Created,
            onFailure: CustomResult.Problem);
    }
}
