using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            return _roleRepository.GetAllAsync();
        }

        public Task<RoleDto?> GetByIdAsync(int id)
        {
            return _roleRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(RoleDto role)
        {
            await ValidateRoleAsync(role, null);
            role.IsActive = true;
            await _roleRepository.AddAsync(role);
        }

        public async Task UpdateAsync(RoleDto role)
        {
            if (role.RoleId <= 0)
            {
                throw new ArgumentException("El ID del rol es invalido.");
            }

            await ValidateRoleAsync(role, role.RoleId);
            await _roleRepository.UpdateAsync(role);
        }

        public Task DeactivateAsync(int id)
        {
            return _roleRepository.DeactivateAsync(id);
        }

        private async Task ValidateRoleAsync(RoleDto role, int? ignoreRoleId)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                throw new ArgumentException("El nombre del rol es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(role.Code))
            {
                throw new ArgumentException("El codigo del rol es obligatorio.");
            }

            if (await _roleRepository.CodeExistsAsync(role.Code, ignoreRoleId))
            {
                throw new ArgumentException("Ya existe un rol con ese codigo.");
            }
        }
    }
}
