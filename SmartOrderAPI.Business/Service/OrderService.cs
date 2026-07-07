using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;

        public OrderService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            return await _orderRepo.GetAllAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetActiveCustomOrdersAsync()
        {
            return await _orderRepo.GetActiveCustomOrdersAsync();
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetCustomOrderSummariesAsync(DateTime startUtc, DateTime endExclusiveUtc)
        {
            if (endExclusiveUtc <= startUtc)
                throw new ArgumentException("El rango de fechas no es valido.");

            return await _orderRepo.GetCustomOrderSummariesAsync(startUtc, endExclusiveUtc);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            return await _orderRepo.GetByIdAsync(id);
        }

        public async Task<OrderDto?> GetInStoreSaleByIdAsync(int id)
        {
            return await _orderRepo.GetInStoreSaleByIdAsync(id);
        }

        public async Task<OrderDto?> GetCustomOrderByIdAsync(int id)
        {
            return await _orderRepo.GetCustomOrderByIdAsync(id);
        }

        public async Task<int> CreateAsync(OrderDto dto)
        {
            if (dto.Pieces <= 0)
                throw new ArgumentException("El número de piezas debe ser mayor a cero.");

            if (dto.TotalAmount <= 0)
                throw new ArgumentException("El monto total debe ser mayor a cero.");

            dto.CreatedAt = DateTime.UtcNow;

            await _orderRepo.AddAsync(dto);
            return dto.OrderId;
        }

        public async Task UpdateAsync(OrderDto dto)
        {
            if (dto.OrderId <= 0)
                throw new ArgumentException("ID de orden inválido.");

            dto.UpdatedAt = DateTime.UtcNow;
            await _orderRepo.UpdateAsync(dto);
        }

        public async Task UpdateStatusAsync(int id, OrderStatusUpdateDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("ID de orden invalido.");

            if (string.IsNullOrWhiteSpace(dto.OrderStatusCode))
                throw new ArgumentException("El estatus del pedido es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.PaymentStatusCode))
                throw new ArgumentException("El estatus de pago es obligatorio.");

            await _orderRepo.UpdateStatusAsync(id, dto);
        }

        public async Task CancelAsync(int id)
        {
            await _orderRepo.DeleteAsync(id);
        }
    }
}
