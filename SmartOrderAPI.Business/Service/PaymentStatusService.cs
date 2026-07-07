using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class PaymentStatusService : IPaymentStatusService
    {
        private readonly IPaymentStatusRepository _paymentStatusRepo;

        public PaymentStatusService(IPaymentStatusRepository paymentStatusRepo)
        {
            _paymentStatusRepo = paymentStatusRepo;
        }

        public async Task<IEnumerable<PaymentStatusDto>> GetAllAsync()
        {
            return await _paymentStatusRepo.GetAllAsync();
        }

        public async Task<PaymentStatusDto?> GetByCodeAsync(string code)
        {
            return await _paymentStatusRepo.GetByCodeAsync(code);
        }

        public async Task CreateAsync(PaymentStatusDto dto)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(dto.PaymentStatusCode))
                throw new ArgumentException("El código de estado de pago es obligatorio.");

            await _paymentStatusRepo.AddAsync(dto);
        }

        public async Task UpdateAsync(PaymentStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PaymentStatusCode))
                throw new ArgumentException("Código inválido para actualizar.");

            await _paymentStatusRepo.UpdateAsync(dto);
        }

        public async Task DeleteAsync(string code)
        {
            await _paymentStatusRepo.DeleteAsync(code);
        }
    }
}
