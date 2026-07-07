using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IUserBranchService
    {
        Task<IEnumerable<UserBranchDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserBranchDto>> GetByBranchIdAsync(int branchId);
        Task<UserBranchDto?> GetByIdAsync(int id);
        Task<int> AssignAsync(UserBranchDto dto);
        Task UnassignAsync(int id);
    }
}
