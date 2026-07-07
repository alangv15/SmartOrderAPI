using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<IEnumerable<OrderDto>> GetActiveCustomOrdersAsync();
        Task<IEnumerable<OrderSummaryDto>> GetCustomOrderSummariesAsync(DateTime startUtc, DateTime endExclusiveUtc);
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto?> GetInStoreSaleByIdAsync(int id);
        Task<OrderDto?> GetCustomOrderByIdAsync(int id);
        Task AddAsync(OrderDto orderDto);
        Task UpdateAsync(OrderDto orderDto);
        Task UpdateStatusAsync(int id, OrderStatusUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
