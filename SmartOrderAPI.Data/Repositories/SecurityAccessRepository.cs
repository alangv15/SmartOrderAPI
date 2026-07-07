using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public class SecurityAccessRepository : ISecurityAccessRepository
    {
        private readonly SmartOrderContext _context;

        public SecurityAccessRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<RoleAccessDto?> GetRoleAccessAsync(int roleId)
        {
            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.RoleId == roleId);

            if (role == null)
            {
                return null;
            }

            var assignedPermissionIds = await _context.RolePermissions
                .AsNoTracking()
                .Where(item => item.RoleId == roleId)
                .Select(item => item.PermissionId)
                .ToListAsync();

            var permissions = await _context.Permissions
                .AsNoTracking()
                .OrderBy(permission => permission.Module)
                .ThenBy(permission => permission.Name)
                .ToListAsync();

            return new RoleAccessDto
            {
                RoleId = role.RoleId,
                RoleCode = role.Code,
                RoleName = role.Name,
                Permissions = permissions
                    .Select(permission => permission.ToDto(assignedPermissionIds.Contains(permission.PermissionId)))
                    .ToList()
            };
        }

        public async Task UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissionIds)
        {
            var roleExists = await _context.Roles.AnyAsync(role => role.RoleId == roleId);
            if (!roleExists)
            {
                throw new ArgumentException("El rol seleccionado no existe.");
            }

            var requestedIds = permissionIds
                .Distinct()
                .ToList();

            var validPermissionIds = await _context.Permissions
                .Where(permission => requestedIds.Contains(permission.PermissionId))
                .Select(permission => permission.PermissionId)
                .ToListAsync();

            if (requestedIds.Except(validPermissionIds).Any())
            {
                throw new ArgumentException("Hay permisos seleccionados que no existen.");
            }

            var existingAssignments = await _context.RolePermissions
                .Where(item => item.RoleId == roleId)
                .ToListAsync();

            var assignmentsToRemove = existingAssignments
                .Where(item => !requestedIds.Contains(item.PermissionId))
                .ToList();

            if (assignmentsToRemove.Count > 0)
            {
                _context.RolePermissions.RemoveRange(assignmentsToRemove);
            }

            var currentPermissionIds = existingAssignments
                .Select(item => item.PermissionId)
                .ToHashSet();

            var assignmentsToAdd = requestedIds
                .Where(permissionId => !currentPermissionIds.Contains(permissionId))
                .Select(permissionId => new Data.Models.RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    CreatedAt = DateTime.UtcNow
                })
                .ToList();

            if (assignmentsToAdd.Count > 0)
            {
                _context.RolePermissions.AddRange(assignmentsToAdd);
            }

            await _context.SaveChangesAsync();
        }
    }
}
