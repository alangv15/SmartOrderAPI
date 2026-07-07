using System;
using System.Collections.Generic;

namespace SmartOrderAPI.Data.Models;

public partial class OrderDiscount
{
    public int OrderDiscountId { get; set; }

    public int OrderId { get; set; }

    public int DiscountRuleId { get; set; }

    public decimal AppliedAmount { get; set; }

    public DateTime AppliedDate { get; set; }

    public virtual DiscountRule DiscountRule { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
