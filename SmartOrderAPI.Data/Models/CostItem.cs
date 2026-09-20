namespace SmartOrderAPI.Data.Models;

public partial class CostItem
{
    public int CostItemId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string CostItemTypeCode { get; set; } = "RawMaterial";

    public string UnitCode { get; set; } = "Gram";

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CostItemCost> CostItemCosts { get; set; } = new List<CostItemCost>();
}
