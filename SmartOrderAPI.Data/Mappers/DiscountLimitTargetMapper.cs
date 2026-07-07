using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers;

public static class DiscountLimitTargetMapper
{
    public static DiscountLimitTargetDto ToDto(DiscountLimitTarget entity) => new()
    {
        DiscountLimitTargetId = entity.DiscountLimitTargetId,
        DiscountLimitRuleId = entity.DiscountLimitRuleId,
        TargetType = entity.TargetType,
        TargetId = entity.TargetId
    };

    public static DiscountLimitTarget ToEntity(DiscountLimitTargetDto dto) => new()
    {
        DiscountLimitTargetId = dto.DiscountLimitTargetId,
        DiscountLimitRuleId = dto.DiscountLimitRuleId,
        TargetType = dto.TargetType,
        TargetId = dto.TargetId
    };
}
