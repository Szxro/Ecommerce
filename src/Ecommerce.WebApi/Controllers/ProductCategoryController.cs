using Ecommerce.Application.Features.ProductCategories.Commands.CreateProductCategory;
using Ecommerce.Application.Features.ProductCategories.Commands.DeleteProductCategoryCommand;
using Ecommerce.Application.Features.ProductCategories.Commands.UpdateProductCategory;
using Ecommerce.Application.Features.ProductCategories.Queries.GetProductCategoriesQuery;
using Ecommerce.Application.Common.DTOs.Response.Queries;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.WebApi.Common;
using Ecommerce.WebApi.Extensions;
using Ecommerce.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers;

[Route("api/product-category")]
[Authorize]
[IsAdminFilter]
[ApiController]
public class ProductCategoryController : ControllerBase
{
    private readonly ISender _sender;

    public ProductCategoryController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IResult> GetAllProductCategory()
    {
        Result<List<ProductCategoryResponse>> result = await _sender.Send(new GetProductCategoriesQuery());

        return result.Match(
            onSuccess: () => CustomResult.Success(result),
            onFailure: CustomResult.Problem);
    }

    [HttpPost("{category}")]
    public async Task<IResult> CreateProductCategory(string category)
    {
        Result result = await _sender.Send(new CreateProductCategoryCommand(category));

        return result.Match(
            onSuccess: Results.Created,
            onFailure: CustomResult.Problem);
    }

    [HttpPut]
    public async Task<IResult> UpdateProductCategory(UpdateProductCategoryCommand updateProduct)
    {
        Result result = await _sender.Send(updateProduct);

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }

    [HttpDelete("{categoryName}")]
    public async Task<IResult> DeleteProductCategory(string categoryName)
    {
        Result result = await _sender.Send(new DeleteProductCategoryCommand(categoryName));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: CustomResult.Problem);
    }
}
