using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartOrderAPI.Data.Repositories
{
    public class UserBranchRepository : IUserBranchRepository
    {
        private readonly SmartOrderContext _context;

        public UserBranchRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserBranchDto>> GetByUserIdAsync(int userId)
        {
            return await _context.UserBranches
                .Where(ub => ub.UserId == userId)
                .Select(ub => ub.ToDto())
                .ToListAsync();
        }

        public async Task<IEnumerable<UserBranchDto>> GetByBranchIdAsync(int branchId)
        {
            return await _context.UserBranches
                .Where(ub => ub.BranchId == branchId)
                .Select(ub => ub.ToDto())
                .ToListAsync();
        }

        public async Task<UserBranchDto?> GetByIdAsync(int id)
        {
            var entity = await _context.UserBranches.FindAsync(id);
            return entity?.ToDto();
        }

        public async Task AddAsync(UserBranchDto dto)
        {
            var entity = dto.ToEntity();
            entity.AssignedAt = DateTime.UtcNow;

            _context.UserBranches.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.UserBranches.FindAsync(id);
            if (entity != null)
            {
                _context.UserBranches.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
