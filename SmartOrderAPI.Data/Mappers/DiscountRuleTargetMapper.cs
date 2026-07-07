using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Mappers
{
    public static class DiscountRuleTargetMapper
    {
        public static DiscountRuleTargetDto ToDto(DiscountRuleTarget entity) => new()
        {
            DiscountRuleTargetId = entity.DiscountRuleTargetId,
            DiscountRuleId = entity.DiscountRuleId,
            TargetType = entity.TargetType,
            TargetId = entity.TargetId
        };

        public static DiscountRuleTarget ToEntity(DiscountRuleTargetDto dto) => new()
        {
            DiscountRuleTargetId = dto.DiscountRuleTargetId,
            DiscountRuleId = dto.DiscountRuleId,
            TargetType = dto.TargetType,
            TargetId = dto.TargetId
        };
    }
}
