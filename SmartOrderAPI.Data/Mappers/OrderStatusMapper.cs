using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class OrderStatusMapper
    {
        public static OrderStatusDto ToDto(this OrderStatus entity) => new OrderStatusDto
        {
            OrderStatusCode = entity.OrderStatusCode,
            DisplayName = entity.DisplayName,
            IsActive = entity.IsActive
        };

        public static OrderStatus ToEntity(this OrderStatusDto dto) => new OrderStatus
        {
            OrderStatusCode = dto.OrderStatusCode,
            DisplayName = dto.DisplayName,
            IsActive = dto.IsActive
        };
    }
}
