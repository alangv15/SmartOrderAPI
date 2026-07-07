using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SmartOrderContext _context;

        public RoleRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .Include(role => role.Users)
                .OrderByDescending(role => role.IsActive)
                .ThenBy(role => role.Name)
                .Select(role => role.ToDto())
                .ToListAsync();
        }

        public async Task<RoleDto?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .AsNoTracking()
                .Include(role => role.Users)
                .Where(role => role.RoleId == id)
                .Select(role => role.ToDto())
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(RoleDto role)
        {
            var entity = new Role
            {
                Name = role.Name.Trim(),
                Code = role.Code.Trim().ToUpperInvariant(),
                Description = string.IsNullOrWhiteSpace(role.Description) ? null : role.Description.Trim(),
                IsActive = role.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoleDto role)
        {
            var entity = await _context.Roles.FindAsync(role.RoleId);
            if (entity == null)
            {
                return;
            }

            entity.Name = role.Name.Trim();
            entity.Code = role.Code.Trim().ToUpperInvariant();
            entity.Description = string.IsNullOrWhiteSpace(role.Description) ? null : role.Description.Trim();
            entity.IsActive = role.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(int id)
        {
            var entity = await _context.Roles.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public Task<bool> CodeExistsAsync(string code, int? ignoreRoleId = null)
        {
            var normalizedCode = code.Trim().ToUpperInvariant();
            return _context.Roles.AnyAsync(role =>
                role.Code == normalizedCode &&
                (!ignoreRoleId.HasValue || role.RoleId != ignoreRoleId.Value));
        }
    }
}
