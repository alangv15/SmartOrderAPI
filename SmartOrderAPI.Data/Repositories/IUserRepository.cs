using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> ValidateUserAsync(string email, string password);
        Task<List<string>> GetPermissionCodesByRoleIdAsync(int roleId);
        Task AddAsync(UserDto user);
        Task UpdateAsync(UserDto dto);
        Task DeleteAsync(int id);
        Task<bool> RoleExistsAsync(int roleId);
    }
}
