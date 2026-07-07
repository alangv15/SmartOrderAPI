using SmartOrderAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IDiscountRuleTargetRepository
    {
        Task<List<DiscountRuleTarget>> GetByRuleIdAsync(int discountRuleId);
        Task AddAsync(DiscountRuleTarget target);
        Task DeleteAsync(int discountRuleTargetId);
    }
}
