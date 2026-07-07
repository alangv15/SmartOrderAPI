using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Business.Service
{
    public interface IDiscountRuleTargetService
    {
        Task<List<DiscountRuleTargetDto>> GetByRuleIdAsync(int discountRuleId);
        Task AddAsync(DiscountRuleTargetDto dto);
        Task DeleteAsync(int discountRuleTargetId);
    }
}
