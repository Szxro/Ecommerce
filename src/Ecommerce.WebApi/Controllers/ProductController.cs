using Ecommerce.Application.Features.Products.Commands.CreateProductCommand;
using Ecommerce.WebApi.Filters;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.Application.Common.DTOs.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.WebApi.Extensions;
using Ecommerce.WebApi.Common;
using Ecommerce.Application.Features.Products.Commands.UpdateProductCommand;

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
}
