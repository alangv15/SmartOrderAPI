using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Entities.Models
{
    public class DiscountRuleDto
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
        public ICollection<DiscountRuleTargetDto> DiscountRuleTargets { get; set; } = new List<DiscountRuleTargetDto>();
        public DiscountTargetDto DiscountTargetCodeNavigation { get; set; } = null!;
        public DiscountTypeDto DiscountTypeCodeNavigation { get; set; } = null!;
    }
}
