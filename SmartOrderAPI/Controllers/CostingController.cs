using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CostingController : ControllerBase
{
    private readonly ICostingService _service;

    public CostingController(ICostingService service)
    {
        _service = service;
    }

    [HttpGet("cost-items")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CostItemDto>>>> GetCostItems()
    {
        return Ok(new ApiResponse<IEnumerable<CostItemDto>>(await _service.GetCostItemsAsync()));
    }

    [HttpGet("cost-items/{id:int}")]
    public async Task<ActionResult<ApiResponse<CostItemDto>>> GetCostItem(int id)
    {
        var costItem = await _service.GetCostItemAsync(id);
        return costItem == null
            ? NotFound(new ApiResponse<CostItemDto>(new Exception("Concepto no encontrado.")))
            : Ok(new ApiResponse<CostItemDto>(costItem));
    }

    [HttpPost("cost-items")]
    public async Task<ActionResult<ApiResponse<int>>> SaveCostItem([FromBody] SaveCostItemRequestDto request)
    {
        try
        {
            var id = await _service.SaveCostItemAsync(request);
            return Ok(new ApiResponse<int>(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<int>(ex, "Error de validacion al guardar el concepto."));
        }
    }

    [HttpGet("recipes")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductRecipeDto>>>> GetRecipes()
    {
        return Ok(new ApiResponse<IEnumerable<ProductRecipeDto>>(await _service.GetRecipesAsync()));
    }

    [HttpGet("base-recipes")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BaseRecipeDto>>>> GetBaseRecipes()
    {
        return Ok(new ApiResponse<IEnumerable<BaseRecipeDto>>(await _service.GetBaseRecipesAsync()));
    }

    [HttpPost("base-recipes")]
    public async Task<ActionResult<ApiResponse<int>>> SaveBaseRecipe([FromBody] SaveBaseRecipeRequestDto request)
    {
        try
        {
            return Ok(new ApiResponse<int>(await _service.SaveBaseRecipeAsync(request)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<int>(ex, "No se pudo guardar la receta base."));
        }
    }

    [HttpGet("recipes/current/{productId:int}")]
    public async Task<ActionResult<ApiResponse<ProductRecipeDto>>> GetCurrentRecipe(int productId)
    {
        var recipe = await _service.GetCurrentRecipeAsync(productId);
        return recipe == null
            ? NotFound(new ApiResponse<ProductRecipeDto>(new Exception("Receta no encontrada.")))
            : Ok(new ApiResponse<ProductRecipeDto>(recipe));
    }

    [HttpPost("recipes")]
    public async Task<ActionResult<ApiResponse<int>>> SaveRecipe([FromBody] SaveProductRecipeRequestDto request)
    {
        try
        {
            var id = await _service.SaveRecipeAsync(request);
            return Ok(new ApiResponse<int>(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<int>(ex, "Error de validacion al guardar la receta."));
        }
    }

    [HttpGet("products/issues")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductCostingIssueDto>>>> GetProductIssues()
    {
        return Ok(new ApiResponse<IEnumerable<ProductCostingIssueDto>>(await _service.GetProductsWithoutCostingAsync()));
    }
}
