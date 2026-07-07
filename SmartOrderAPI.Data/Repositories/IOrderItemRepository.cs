using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItemDto>> GetByOrderIdAsync(int orderId);
        Task<OrderItemDto?> GetByIdAsync(int id);
        Task AddAsync(OrderItemDto dto);
        Task UpdateAsync(OrderItemDto dto);
        Task DeleteAsync(int id);
    }
}
