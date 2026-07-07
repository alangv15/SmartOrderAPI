namespace SmartOrderAPI.Data.Models;

public partial class DiscountLimitRule
{
    public int DiscountLimitRuleId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal MaxDiscountPerUnit { get; set; }
    public string AdjustmentTypeCode { get; set; } = "CapAmount";
    public decimal AdjustmentValue { get; set; }
    public bool IsActive { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public virtual ICollection<DiscountLimitTarget> DiscountLimitTargets { get; set; } = new List<DiscountLimitTarget>();
}
