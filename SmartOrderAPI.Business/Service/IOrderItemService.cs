using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetByOrderIdAsync(int orderId);
        Task<OrderItemDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(OrderItemDto dto);
        Task UpdateAsync(OrderItemDto dto);
        Task DeleteAsync(int id);
    }
}
