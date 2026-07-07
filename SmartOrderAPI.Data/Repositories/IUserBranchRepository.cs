using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IUserBranchRepository
    {
        Task<IEnumerable<UserBranchDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserBranchDto>> GetByBranchIdAsync(int branchId);
        Task<UserBranchDto?> GetByIdAsync(int id);
        Task AddAsync(UserBranchDto dto);
        Task DeleteAsync(int id);
    }
}
