using Ecommerce.Application.Features.ProductCategories.Commands.CreateProductCategory;
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

    [HttpPost("{category}")]
    public async Task<IResult> CreateProductCategory(string category)
    {
        Result result = await _sender.Send(new CreateProductCategoryCommand(category));

        return result.Match(
            onSuccess: Results.Created,
            onFailure: CustomResult.Problem);
    }
}
