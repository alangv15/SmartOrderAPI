using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Mappers
{
    public static class OrderDiscountMapper
    {
        public static OrderDiscountDto ToDto(OrderDiscount entity) => new()
        {
            OrderDiscountId = entity.OrderDiscountId,
            OrderId = entity.OrderId,
            DiscountRuleId = entity.DiscountRuleId,
            AppliedAmount = entity.AppliedAmount,
            AppliedDate = entity.AppliedDate
        };

        public static OrderDiscount ToEntity(OrderDiscountDto dto) => new()
        {
            OrderDiscountId = dto.OrderDiscountId,
            OrderId = dto.OrderId,
            DiscountRuleId = dto.DiscountRuleId,
            AppliedAmount = dto.AppliedAmount,
            AppliedDate = dto.AppliedDate
        };
    }
}
