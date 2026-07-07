using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IBranchRepository
    {
        Task<IEnumerable<BranchDto>> GetAllAsync();
        Task<BranchDto?> GetByIdAsync(int id);
        Task AddAsync(BranchDto branchDto);
        Task UpdateAsync(BranchDto branchDto);
        Task DeleteAsync(int id);
    }
}
