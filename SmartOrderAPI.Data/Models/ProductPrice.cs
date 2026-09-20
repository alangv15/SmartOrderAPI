namespace SmartOrderAPI.Data.Models;

public partial class ProductPrice
{
    public int ProductPriceId { get; set; }

    public int ProductId { get; set; }

    public decimal SalePrice { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public string? Notes { get; set; }

    public virtual Product Product { get; set; } = null!;
}
