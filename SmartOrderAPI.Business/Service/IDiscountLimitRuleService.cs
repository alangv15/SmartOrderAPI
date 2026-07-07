using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service;

public interface IDiscountLimitRuleService
{
    Task<List<DiscountLimitRuleDto>> GetAllAsync();
    Task<DiscountLimitRuleDto?> GetByIdAsync(int id);
    Task CreateAsync(DiscountLimitRuleDto dto);
    Task UpdateAsync(DiscountLimitRuleDto dto);
    Task DeleteAsync(int id);
}
