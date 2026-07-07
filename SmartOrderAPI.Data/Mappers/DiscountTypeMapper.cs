using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class DiscountTypeMapper
    {
        public static DiscountTypeDto ToDto(this DiscountType entity) => new DiscountTypeDto
        {
            DiscountTypeCode = entity.DiscountTypeCode,
            DisplayName = entity.DisplayName,
            IsActive = entity.IsActive
        };

        public static DiscountType ToEntity(this DiscountTypeDto dto) => new DiscountType
        {
            DiscountTypeCode = dto.DiscountTypeCode,
            DisplayName = dto.DisplayName,
            IsActive = dto.IsActive
        };
    }
}
