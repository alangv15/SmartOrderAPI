using System;
using System.Collections.Generic;

namespace SmartOrderAPI.Data.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal UnitCost { get; set; }

    public decimal? DiscountPerUnit { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? LineTotal { get; set; }

    public decimal? CostAmount { get; set; }

    public decimal? GrossProfit { get; set; }

    public int? ProductRecipeId { get; set; }

    public int? ProductPriceId { get; set; }

    public DateTime? CostCalculatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ProductRecipe? ProductRecipe { get; set; }

    public virtual ProductPrice? ProductPrice { get; set; }
}
