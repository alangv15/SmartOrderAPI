using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _userRepo.GetAllAsync();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await _userRepo.GetByIdAsync(id);
        }

        public async Task<UserDto?> ValidateUserAsync(string email, string password)
        {
            return await _userRepo.ValidateUserAsync(email, password);
        }

        public async Task<List<string>> GetPermissionCodesByRoleIdAsync(int roleId)
        {
            return await _userRepo.GetPermissionCodesByRoleIdAsync(roleId);
        }

        public async Task<bool> CreateAsync(UserDto user)
        {
            await ValidateUserDataAsync(user, requiresPassword: true);
            user.Email = user.Email.Trim();
            user.IsActive = true;
            await _userRepo.AddAsync(user);
            return true;
        }

        public async Task UpdateAsync(UserDto dto)
        {
            if (dto.UserId <= 0)
            {
                throw new ArgumentException("ID inválido para actualizar.");
            }

            await ValidateUserDataAsync(dto, requiresPassword: false);
            dto.Email = dto.Email.Trim();
            await _userRepo.UpdateAsync(dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepo.DeleteAsync(id);
        }

        private async Task ValidateUserDataAsync(UserDto user, bool requiresPassword)
        {
            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new ArgumentException("El nombre completo es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("El correo electrónico es obligatorio.");
            }

            if (user.RoleId <= 0)
            {
                throw new ArgumentException("El rol es obligatorio.");
            }

            if (!await _userRepo.RoleExistsAsync(user.RoleId))
            {
                throw new ArgumentException("El rol seleccionado no existe o no esta activo.");
            }

            if (requiresPassword && string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Las credenciales son obligatorias.");
            }
        }
    }
}
