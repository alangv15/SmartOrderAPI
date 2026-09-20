using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItemDto ToDto(this OrderItem entity) => new OrderItemDto
        {
            OrderItemId = entity.OrderItemId,
            OrderId = entity.OrderId,
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            UnitPrice = entity.UnitPrice,
            UnitCost = entity.UnitCost,
            DiscountPerUnit = entity.DiscountPerUnit,
            DiscountAmount = entity.DiscountAmount,
            LineTotal = entity.LineTotal,
            CostAmount = entity.CostAmount,
            GrossProfit = entity.GrossProfit,
            ProductRecipeId = entity.ProductRecipeId,
            ProductPriceId = entity.ProductPriceId,
            CostCalculatedAt = entity.CostCalculatedAt
        };

        public static OrderItem ToEntity(this OrderItemDto dto) => new OrderItem
        {
            OrderItemId = dto.OrderItemId,
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            UnitCost = dto.UnitCost,
            DiscountPerUnit = dto.DiscountPerUnit,
            DiscountAmount = dto.DiscountAmount,
            ProductRecipeId = dto.ProductRecipeId,
            ProductPriceId = dto.ProductPriceId,
            CostCalculatedAt = dto.CostCalculatedAt
        };
    }
}
