using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public class DiscountTypeRepository : IDiscountTypeRepository
    {
        private readonly SmartOrderContext _context;

        public DiscountTypeRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiscountTypeDto>> GetAllAsync()
        {
            return await _context.DiscountTypes
                .Where(d => d.IsActive)
                .Select(d => d.ToDto())
                .ToListAsync();
        }

        public async Task<DiscountTypeDto?> GetByCodeAsync(string code)
        {
            var entity = await _context.DiscountTypes
                .FirstOrDefaultAsync(d => d.DiscountTypeCode == code && d.IsActive);

            return entity?.ToDto();
        }

        public async Task AddAsync(DiscountTypeDto dto)
        {
            var entity = dto.ToEntity();
            entity.IsActive = true;

            _context.DiscountTypes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiscountTypeDto dto)
        {
            var existing = await _context.DiscountTypes.FindAsync(dto.DiscountTypeCode);
            if (existing != null && existing.IsActive)
            {
                _context.Entry(existing).CurrentValues.SetValues(dto.ToEntity());
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string code)
        {
            var entity = await _context.DiscountTypes.FindAsync(code);
            if (entity != null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
