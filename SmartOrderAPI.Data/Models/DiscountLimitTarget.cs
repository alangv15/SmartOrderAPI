namespace SmartOrderAPI.Data.Models;

public partial class DiscountLimitTarget
{
    public int DiscountLimitTargetId { get; set; }
    public int DiscountLimitRuleId { get; set; }
    public string TargetType { get; set; } = null!;
    public int TargetId { get; set; }
    public virtual DiscountLimitRule DiscountLimitRule { get; set; } = null!;
}
