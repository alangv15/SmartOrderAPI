using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Mappers
{
    public static class DiscountTargetMapper
    {
        public static DiscountTargetDto ToDto(DiscountTarget entity) => new()
        {
            DiscountTargetCode = entity.DiscountTargetCode,
            DisplayName = entity.DisplayName,
            IsActive = entity.IsActive
        };

        public static DiscountTarget ToEntity(DiscountTargetDto dto) => new()
        {
            DiscountTargetCode = dto.DiscountTargetCode,
            DisplayName = dto.DisplayName,
            IsActive = dto.IsActive
        };
    }
}
