using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IDiscountTypeRepository
    {
        Task<IEnumerable<DiscountTypeDto>> GetAllAsync();
        Task<DiscountTypeDto?> GetByCodeAsync(string code);
        Task AddAsync(DiscountTypeDto dto);
        Task UpdateAsync(DiscountTypeDto dto);
        Task DeleteAsync(string code);
    }
}
