using System;
using System.Collections.Generic;

namespace SmartOrderAPI.Data.Models;

public partial class DiscountRuleTarget
{
    public int DiscountRuleTargetId { get; set; }

    public int DiscountRuleId { get; set; }

    public string TargetType { get; set; } = null!;

    public int TargetId { get; set; }

    public virtual DiscountRule DiscountRule { get; set; } = null!;
}
