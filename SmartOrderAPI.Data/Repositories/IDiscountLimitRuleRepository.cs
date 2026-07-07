using SmartOrderAPI.Data.Models;

namespace SmartOrderAPI.Data.Repositories;

public interface IDiscountLimitRuleRepository
{
    Task<List<DiscountLimitRule>> GetAllAsync();
    Task<DiscountLimitRule?> GetByIdAsync(int id);
    Task AddAsync(DiscountLimitRule rule);
    Task UpdateAsync(DiscountLimitRule rule);
    Task DeleteAsync(int id);
}
