using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartOrderAPI.Data.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly SmartOrderContext _context;

        public OrderStatusRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderStatusDto>> GetAllAsync()
        {
            return await _context.OrderStatuses
                .Where(s => s.IsActive)
                .Select(s => s.ToDto())
                .ToListAsync();
        }

        public async Task<OrderStatusDto?> GetByCodeAsync(string code)
        {
            var entity = await _context.OrderStatuses
                .FirstOrDefaultAsync(s => s.OrderStatusCode == code && s.IsActive);

            return entity?.ToDto();
        }

        public async Task AddAsync(OrderStatusDto dto)
        {
            var entity = dto.ToEntity();
            entity.IsActive = true;

            _context.OrderStatuses.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderStatusDto dto)
        {
            var existing = await _context.OrderStatuses.FindAsync(dto.OrderStatusCode);
            if (existing != null && existing.IsActive)
            {
                _context.Entry(existing).CurrentValues.SetValues(dto.ToEntity());
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string code)
        {
            var entity = await _context.OrderStatuses.FindAsync(code);
            if (entity != null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
