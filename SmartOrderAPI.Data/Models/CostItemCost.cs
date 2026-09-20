namespace SmartOrderAPI.Data.Models;

public partial class CostItemCost
{
    public int CostItemCostId { get; set; }

    public int CostItemId { get; set; }

    public string PresentationName { get; set; } = null!;

    public decimal PresentationQuantity { get; set; }

    public decimal PresentationCost { get; set; }

    public decimal? UnitCost { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public string? Notes { get; set; }

    public virtual CostItem CostItem { get; set; } = null!;
}
