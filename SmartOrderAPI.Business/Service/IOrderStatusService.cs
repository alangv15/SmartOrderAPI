using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IOrderStatusService
    {
        Task<IEnumerable<OrderStatusDto>> GetAllAsync();
        Task<OrderStatusDto?> GetByCodeAsync(string code);
        Task CreateAsync(OrderStatusDto dto);
        Task UpdateAsync(OrderStatusDto dto);
        Task DeleteAsync(string code);
    }
}
