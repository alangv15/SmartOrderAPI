using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface ISecurityAccessRepository
    {
        Task<RoleAccessDto?> GetRoleAccessAsync(int roleId);
        Task UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissionIds);
    }
}
