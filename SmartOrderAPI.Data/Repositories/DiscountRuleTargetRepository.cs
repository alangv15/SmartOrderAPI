using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public class DiscountRuleTargetRepository : IDiscountRuleTargetRepository
    {
        private readonly SmartOrderContext _context;

        public DiscountRuleTargetRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<List<DiscountRuleTarget>> GetByRuleIdAsync(int discountRuleId) =>
            await _context.DiscountRuleTargets
                .Where(t => t.DiscountRuleId == discountRuleId)
                .ToListAsync();

        public async Task AddAsync(DiscountRuleTarget target)
        {
            _context.DiscountRuleTargets.Add(target);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int discountRuleTargetId)
        {
            var target = await _context.DiscountRuleTargets.FindAsync(discountRuleTargetId);
            if (target is null) return;
            _context.DiscountRuleTargets.Remove(target);
            await _context.SaveChangesAsync();
        }
    }
}
