using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepo;

        public OrderItemService(IOrderItemRepository orderItemRepo)
        {
            _orderItemRepo = orderItemRepo;
        }

        public async Task<IEnumerable<OrderItemDto>> GetByOrderIdAsync(int orderId)
        {
            return await _orderItemRepo.GetByOrderIdAsync(orderId);
        }

        public async Task<OrderItemDto?> GetByIdAsync(int id)
        {
            return await _orderItemRepo.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(OrderItemDto dto)
        {
            // Validaciones de negocio
            if (dto.Quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");

            if (dto.UnitPrice < 0)
                throw new ArgumentException("El precio unitario no puede ser negativo.");

            // Puedes calcular el descuento total si no viene
            if (dto.DiscountAmount == null && dto.DiscountPerUnit != null)
                dto.DiscountAmount = dto.Quantity * dto.DiscountPerUnit;

            await _orderItemRepo.AddAsync(dto);
            return dto.OrderItemId;
        }

        public async Task UpdateAsync(OrderItemDto dto)
        {
            if (dto.OrderItemId <= 0)
                throw new ArgumentException("ID inválido para actualizar.");

            await _orderItemRepo.UpdateAsync(dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _orderItemRepo.DeleteAsync(id);
        }
    }
}
