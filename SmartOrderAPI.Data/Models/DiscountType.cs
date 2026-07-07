
using System;
using System.Collections.Generic;

namespace SmartOrderAPI.Data.Models;

public partial class DiscountType
{
    public string DiscountTypeCode { get; set; } = null!;

    public string? DisplayName { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<DiscountRule> DiscountRules { get; set; } = new List<DiscountRule>();
}
