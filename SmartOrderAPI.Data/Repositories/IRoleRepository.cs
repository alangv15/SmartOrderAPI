using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(int id);
        Task AddAsync(RoleDto role);
        Task UpdateAsync(RoleDto role);
        Task DeactivateAsync(int id);
        Task<bool> CodeExistsAsync(string code, int? ignoreRoleId = null);
    }
}
