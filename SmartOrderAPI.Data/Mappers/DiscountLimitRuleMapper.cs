using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers;

public static class DiscountLimitRuleMapper
{
    public static DiscountLimitRuleDto ToDto(DiscountLimitRule entity) => new()
    {
        DiscountLimitRuleId = entity.DiscountLimitRuleId,
        Name = entity.Name,
        Description = entity.Description,
        MaxDiscountPerUnit = entity.MaxDiscountPerUnit,
        AdjustmentTypeCode = entity.AdjustmentTypeCode,
        AdjustmentValue = entity.AdjustmentValue,
        IsActive = entity.IsActive,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        DiscountLimitTargets = entity.DiscountLimitTargets
            .Select(DiscountLimitTargetMapper.ToDto)
            .ToList()
    };

    public static DiscountLimitRule ToEntity(DiscountLimitRuleDto dto) => new()
    {
        DiscountLimitRuleId = dto.DiscountLimitRuleId,
        Name = dto.Name,
        Description = dto.Description,
        MaxDiscountPerUnit = dto.MaxDiscountPerUnit,
        AdjustmentTypeCode = string.IsNullOrWhiteSpace(dto.AdjustmentTypeCode) ? "CapAmount" : dto.AdjustmentTypeCode,
        AdjustmentValue = dto.AdjustmentValue <= 0 ? dto.MaxDiscountPerUnit : dto.AdjustmentValue,
        IsActive = dto.IsActive,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        DiscountLimitTargets = dto.DiscountLimitTargets
            .Select(DiscountLimitTargetMapper.ToEntity)
            .ToList()
    };
}
