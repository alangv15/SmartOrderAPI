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
            DiscountPerUnit = entity.DiscountPerUnit,
            DiscountAmount = entity.DiscountAmount,
            LineTotal = entity.LineTotal
        };

        public static OrderItem ToEntity(this OrderItemDto dto) => new OrderItem
        {
            OrderItemId = dto.OrderItemId,
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            DiscountPerUnit = dto.DiscountPerUnit,
            DiscountAmount = dto.DiscountAmount,
            LineTotal = dto.LineTotal
        };
    }
}
