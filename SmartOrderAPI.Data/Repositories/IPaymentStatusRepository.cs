using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IPaymentStatusRepository
    {
        Task<IEnumerable<PaymentStatusDto>> GetAllAsync();
        Task<PaymentStatusDto?> GetByCodeAsync(string code);
        Task AddAsync(PaymentStatusDto dto);
        Task UpdateAsync(PaymentStatusDto dto);
        Task DeleteAsync(string code);
    }
}
