using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface ISecurityAccessService
    {
        Task<RoleAccessDto?> GetRoleAccessAsync(int roleId);
        Task UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissionIds);
    }
}
