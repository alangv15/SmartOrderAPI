namespace SmartOrderAPI.Data.Models;

public partial class ProductRecipeItem
{
    public int ProductRecipeItemId { get; set; }

    public int ProductRecipeId { get; set; }

    public int CostItemCostId { get; set; }

    public decimal Quantity { get; set; }

    public decimal RecipeItemCost { get; set; }

    public virtual ProductRecipe ProductRecipe { get; set; } = null!;

    public virtual CostItemCost CostItemCost { get; set; } = null!;
}
