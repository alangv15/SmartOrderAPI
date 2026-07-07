using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;

namespace SmartOrderAPI.Data.Repositories;

public class DiscountLimitRuleRepository : IDiscountLimitRuleRepository
{
    private readonly SmartOrderContext _context;

    public DiscountLimitRuleRepository(SmartOrderContext context)
    {
        _context = context;
    }

    public async Task<List<DiscountLimitRule>> GetAllAsync() =>
        await _context.DiscountLimitRules
            .Include(rule => rule.DiscountLimitTargets)
            .Where(rule => rule.IsActive)
            .ToListAsync();

    public async Task<DiscountLimitRule?> GetByIdAsync(int id) =>
        await _context.DiscountLimitRules
            .Include(rule => rule.DiscountLimitTargets)
            .FirstOrDefaultAsync(rule => rule.DiscountLimitRuleId == id);

    public async Task AddAsync(DiscountLimitRule rule)
    {
        _context.DiscountLimitRules.Add(rule);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DiscountLimitRule rule)
    {
        _context.DiscountLimitRules.Update(rule);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var rule = await _context.DiscountLimitRules.FindAsync(id);
        if (rule is null)
        {
            return;
        }

        rule.IsActive = false;
        await _context.SaveChangesAsync();
    }
}
