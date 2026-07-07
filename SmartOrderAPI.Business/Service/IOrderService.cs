using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<IEnumerable<OrderDto>> GetActiveCustomOrdersAsync();
        Task<IEnumerable<OrderSummaryDto>> GetCustomOrderSummariesAsync(DateTime startUtc, DateTime endExclusiveUtc);
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto?> GetInStoreSaleByIdAsync(int id);
        Task<OrderDto?> GetCustomOrderByIdAsync(int id);
        Task<int> CreateAsync(OrderDto dto);
        Task UpdateAsync(OrderDto dto);
        Task UpdateStatusAsync(int id, OrderStatusUpdateDto dto);
        Task CancelAsync(int id);
    }
}
