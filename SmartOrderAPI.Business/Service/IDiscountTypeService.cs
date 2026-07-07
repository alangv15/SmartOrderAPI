using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public interface IDiscountTypeService
    {
        Task<IEnumerable<DiscountTypeDto>> GetAllAsync();
        Task<DiscountTypeDto?> GetByCodeAsync(string code);
        Task CreateAsync(DiscountTypeDto dto);
        Task UpdateAsync(DiscountTypeDto dto);
        Task DeleteAsync(string code);
    }
}
