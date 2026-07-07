using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(int id);
        Task CreateAsync(RoleDto role);
        Task UpdateAsync(RoleDto role);
        Task DeactivateAsync(int id);
    }
}
