using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public class DiscountTargetRepository : IDiscountTargetRepository
    {
        private readonly SmartOrderContext _context;

        public DiscountTargetRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<List<DiscountTarget>> GetAllAsync() =>
            await _context.DiscountTargets.Where(t => t.IsActive).ToListAsync();

        public async Task<DiscountTarget?> GetByCodeAsync(string code) =>
            await _context.DiscountTargets.FindAsync(code);

        public async Task AddAsync(DiscountTarget target)
        {
            _context.DiscountTargets.Add(target);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiscountTarget target)
        {
            _context.DiscountTargets.Update(target);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string code)
        {
            var target = await _context.DiscountTargets.FindAsync(code);
            if (target is null) return;
            target.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
