using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> ValidateUserAsync(string email, string password);
        Task<List<string>> GetPermissionCodesByRoleIdAsync(int roleId);
        Task<bool> CreateAsync(UserDto user);
        Task UpdateAsync(UserDto dto);
        Task DeleteAsync(int id);
    }
}
