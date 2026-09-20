namespace SmartOrderAPI.Data.Models;

public partial class ProductRecipe
{
    public ICollection<ProductRecipeBase> Bases { get; set; } = new List<ProductRecipeBase>();
    public int ProductRecipeId { get; set; }

    public int ProductId { get; set; }

    public int VersionNumber { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public decimal RecipeCost { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public string? Notes { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductRecipeItem> ProductRecipeItems { get; set; } = new List<ProductRecipeItem>();
}
