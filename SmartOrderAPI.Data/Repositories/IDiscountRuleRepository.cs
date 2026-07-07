using SmartOrderAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IDiscountRuleRepository
    {
        Task<List<DiscountRule>> GetAllAsync();
        Task<DiscountRule?> GetByIdAsync(int id);
        Task AddAsync(DiscountRule rule);
        Task UpdateAsync(DiscountRule rule);
        Task DeleteAsync(int id);
    }
}
