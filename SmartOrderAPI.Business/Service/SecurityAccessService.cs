using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class SecurityAccessService : ISecurityAccessService
    {
        private readonly ISecurityAccessRepository _securityAccessRepository;

        public SecurityAccessService(ISecurityAccessRepository securityAccessRepository)
        {
            _securityAccessRepository = securityAccessRepository;
        }

        public Task<RoleAccessDto?> GetRoleAccessAsync(int roleId)
        {
            return _securityAccessRepository.GetRoleAccessAsync(roleId);
        }

        public Task UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissionIds)
        {
            return _securityAccessRepository.UpdateRolePermissionsAsync(roleId, permissionIds);
        }
    }
}
