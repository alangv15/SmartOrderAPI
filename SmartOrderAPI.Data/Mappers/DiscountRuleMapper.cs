using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Mappers
{
    public static class DiscountRuleMapper
    {
        public static DiscountRuleDto ToDto(DiscountRule entity) => new()
        {
            DiscountRuleId = entity.DiscountRuleId,
            Name = entity.Name,
            Description = entity.Description,
            DiscountTypeCode = entity.DiscountTypeCode,
            DiscountValue = entity.DiscountValue,
            DiscountTargetCode = entity.DiscountTargetCode,
            ConditionValue = entity.ConditionValue,
            MinQuantity = entity.MinQuantity,
            MinTotalAmount = entity.MinTotalAmount,
            IsAutomatic = entity.IsAutomatic,
            IsActive = entity.IsActive,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            DiscountRuleTargets = entity.DiscountRuleTargets
                .Select(DiscountRuleTargetMapper.ToDto)
                .ToList(),
            DiscountTargetCodeNavigation = DiscountTargetMapper.ToDto(entity.DiscountTargetCodeNavigation),
            DiscountTypeCodeNavigation = DiscountTypeMapper.ToDto(entity.DiscountTypeCodeNavigation)    
        };

        public static DiscountRule ToEntity(DiscountRuleDto dto) => new()
        {
            DiscountRuleId = dto.DiscountRuleId,
            Name = dto.Name,
            Description = dto.Description,
            DiscountTypeCode = dto.DiscountTypeCode,
            DiscountValue = dto.DiscountValue,
            DiscountTargetCode = dto.DiscountTargetCode,
            ConditionValue = dto.ConditionValue,
            MinQuantity = dto.MinQuantity,
            MinTotalAmount = dto.MinTotalAmount,
            IsAutomatic = dto.IsAutomatic,
            IsActive = dto.IsActive,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            DiscountRuleTargets = dto.DiscountRuleTargets
                .Select(DiscountRuleTargetMapper.ToEntity)
                .ToList(),
            DiscountTargetCodeNavigation = DiscountTargetMapper.ToEntity(dto.DiscountTargetCodeNavigation),
            DiscountTypeCodeNavigation = DiscountTypeMapper.ToEntity(dto.DiscountTypeCodeNavigation)
        };
    }
}
