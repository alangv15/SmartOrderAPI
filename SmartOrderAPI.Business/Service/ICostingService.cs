using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service;

public interface ICostingService
{
    Task<IEnumerable<CostItemDto>> GetCostItemsAsync();
    Task<CostItemDto?> GetCostItemAsync(int costItemId);
    Task<int> SaveCostItemAsync(SaveCostItemRequestDto request);
    Task<IEnumerable<ProductRecipeDto>> GetRecipesAsync();
    Task<IEnumerable<BaseRecipeDto>> GetBaseRecipesAsync();
    Task<int> SaveBaseRecipeAsync(SaveBaseRecipeRequestDto request);
    Task<ProductRecipeDto?> GetCurrentRecipeAsync(int productId);
    Task<int> SaveRecipeAsync(SaveProductRecipeRequestDto request);
    Task<IEnumerable<ProductCostingIssueDto>> GetProductsWithoutCostingAsync();
}
