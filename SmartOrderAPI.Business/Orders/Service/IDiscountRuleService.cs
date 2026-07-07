using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Business.Orders.Service
{
    public interface IDiscountRuleService
    {
        Task<List<DiscountRuleDto>> GetAllAsync();
        Task<DiscountRuleDto?> GetByIdAsync(int id);
        Task CreateAsync(DiscountRuleDto dto);
        Task UpdateAsync(DiscountRuleDto dto);
        Task DeleteAsync(int id);
    }
}
