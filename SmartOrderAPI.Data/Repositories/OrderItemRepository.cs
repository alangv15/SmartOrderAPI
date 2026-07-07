using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly SmartOrderContext _context;

        public OrderItemRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItemDto>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .Select(i => i.ToDto())
                .ToListAsync();
        }

        public async Task<OrderItemDto?> GetByIdAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            return item?.ToDto();
        }

        public async Task AddAsync(OrderItemDto dto)
        {
            var entity = dto.ToEntity();
            _context.OrderItems.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItemDto dto)
        {
            var existing = await _context.OrderItems.FindAsync(dto.OrderItemId);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(dto.ToEntity());
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item != null)
            {
                _context.OrderItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
