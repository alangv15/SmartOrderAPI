using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartOrderAPI.Data.Repositories
{
    public class PaymentStatusRepository : IPaymentStatusRepository
    {
        private readonly SmartOrderContext _context;

        public PaymentStatusRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentStatusDto>> GetAllAsync()
        {
            return await _context.PaymentStatuses
                .Where(p => p.IsActive)
                .Select(p => p.ToDto())
                .ToListAsync();
        }

        public async Task<PaymentStatusDto?> GetByCodeAsync(string code)
        {
            var entity = await _context.PaymentStatuses
                .FirstOrDefaultAsync(p => p.PaymentStatusCode == code && p.IsActive);

            return entity?.ToDto();
        }

        public async Task AddAsync(PaymentStatusDto dto)
        {
            var entity = dto.ToEntity();
            entity.IsActive = true;

            _context.PaymentStatuses.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PaymentStatusDto dto)
        {
            var existing = await _context.PaymentStatuses.FindAsync(dto.PaymentStatusCode);
            if (existing != null && existing.IsActive)
            {
                _context.Entry(existing).CurrentValues.SetValues(dto.ToEntity());
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string code)
        {
            var entity = await _context.PaymentStatuses.FindAsync(code);
            if (entity != null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
