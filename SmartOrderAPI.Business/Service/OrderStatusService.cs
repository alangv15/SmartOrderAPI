using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepository _orderStatusRepo;

        public OrderStatusService(IOrderStatusRepository orderStatusRepo)
        {
            _orderStatusRepo = orderStatusRepo;
        }

        public async Task<IEnumerable<OrderStatusDto>> GetAllAsync()
        {
            return await _orderStatusRepo.GetAllAsync();
        }

        public async Task<OrderStatusDto?> GetByCodeAsync(string code)
        {
            return await _orderStatusRepo.GetByCodeAsync(code);
        }

        public async Task CreateAsync(OrderStatusDto dto)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(dto.OrderStatusCode))
                throw new ArgumentException("El código de estado es obligatorio.");

            await _orderStatusRepo.AddAsync(dto);
        }

        public async Task UpdateAsync(OrderStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OrderStatusCode))
                throw new ArgumentException("Código inválido para actualizar.");

            await _orderStatusRepo.UpdateAsync(dto);
        }

        public async Task DeleteAsync(string code)
        {
            await _orderStatusRepo.DeleteAsync(code);
        }
    }
}
