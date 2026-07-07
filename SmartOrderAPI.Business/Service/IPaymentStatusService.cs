using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IPaymentStatusService
    {
        Task<IEnumerable<PaymentStatusDto>> GetAllAsync();
        Task<PaymentStatusDto?> GetByCodeAsync(string code);
        Task CreateAsync(PaymentStatusDto dto);
        Task UpdateAsync(PaymentStatusDto dto);
        Task DeleteAsync(string code);
    }
}
