using System;
using System.Collections.Generic;

namespace SmartOrderAPI.Data.Models;

public partial class DiscountRule
{
    public int DiscountRuleId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string DiscountTypeCode { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public string DiscountTargetCode { get; set; } = null!;

    public string? ConditionValue { get; set; }

    public int? MinQuantity { get; set; }

    public decimal? MinTotalAmount { get; set; }

    public bool IsAutomatic { get; set; }

    public bool IsActive { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<DiscountRuleTarget> DiscountRuleTargets { get; set; } = new List<DiscountRuleTarget>();

    public virtual DiscountTarget DiscountTargetCodeNavigation { get; set; } = null!;

    public virtual DiscountType DiscountTypeCodeNavigation { get; set; } = null!;

    public virtual ICollection<OrderDiscount> OrderDiscounts { get; set; } = new List<OrderDiscount>();
}
