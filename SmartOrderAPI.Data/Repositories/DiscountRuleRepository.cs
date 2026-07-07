using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public class DiscountRuleRepository : IDiscountRuleRepository
    {
        private readonly SmartOrderContext _context;

        public DiscountRuleRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<List<DiscountRule>> GetAllAsync() =>
            await _context.DiscountRules
                .Include(x => x.DiscountRuleTargets)
                .Include(x => x.DiscountTargetCodeNavigation)
                .Include(x => x.DiscountTypeCodeNavigation)
                .Where(r => r.IsActive).ToListAsync();

        public async Task<DiscountRule?> GetByIdAsync(int id) =>
            await _context.DiscountRules
                .Include(x => x.DiscountRuleTargets)
                .Include(x => x.DiscountTargetCodeNavigation)
                .Include(x => x.DiscountTypeCodeNavigation)
                .FirstOrDefaultAsync(x => x.DiscountRuleId == id);

        public async Task AddAsync(DiscountRule rule)
        {
            _context.DiscountRules.Add(rule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiscountRule rule)
        {
            _context.DiscountRules.Update(rule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rule = await _context.DiscountRules.FindAsync(id);
            if (rule is null) return;
            rule.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
