using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly SmartOrderContext _context;

        public BranchRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BranchDto>> GetAllAsync()
        {
            return await _context.Branches
                .Where(b => b.IsActive)
                .Select(b => b.ToDto())
                .ToListAsync();
        }

        public async Task<BranchDto?> GetByIdAsync(int id)
        {
            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.BranchId == id && b.IsActive);

            return branch?.ToDto();
        }

        public async Task AddAsync(BranchDto branchDto)
        {
            var entity = branchDto.ToEntity();
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;

            _context.Branches.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BranchDto branchDto)
        {
            var existing = await _context.Branches.FindAsync(branchDto.BranchId);
            if (existing != null && existing.IsActive)
            {
                var updated = branchDto.ToEntity();
                _context.Entry(existing).CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                branch.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
