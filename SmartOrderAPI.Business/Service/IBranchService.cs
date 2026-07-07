using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchDto>> GetAllAsync();
        Task<BranchDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(BranchDto dto);
        Task UpdateAsync(BranchDto dto);
        Task DeleteAsync(int id);
    }

}
