namespace SmartOrderAPI.Data.Models;

public sealed class BaseRecipeGroup
{
    public int BaseRecipeGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public sealed class BaseRecipe
{
    public int BaseRecipeId { get; set; }
    public int BaseRecipeGroupId { get; set; }
    public int VersionNumber { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public decimal RecipeCost { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public string? Notes { get; set; }
    public BaseRecipeGroup Group { get; set; } = null!;
    public ICollection<BaseRecipeItem> Items { get; set; } = new List<BaseRecipeItem>();
}

public sealed class BaseRecipeItem
{
    public int BaseRecipeItemId { get; set; }
    public int BaseRecipeId { get; set; }
    public int CostItemCostId { get; set; }
    public decimal Quantity { get; set; }
    public decimal RecipeItemCost { get; set; }
    public CostItemCost CostItemCost { get; set; } = null!;
}

public sealed class ProductRecipeBase
{
    public int ProductRecipeBaseId { get; set; }
    public int ProductRecipeId { get; set; }
    public int BaseRecipeId { get; set; }
    public decimal QuantityMultiplier { get; set; }
    public decimal BaseRecipeCost { get; set; }
    public BaseRecipe BaseRecipe { get; set; } = null!;
}
