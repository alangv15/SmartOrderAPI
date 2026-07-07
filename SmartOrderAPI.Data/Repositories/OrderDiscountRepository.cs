using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public class OrderDiscountRepository : IOrderDiscountRepository
    {
        private readonly SmartOrderContext _context;

        public OrderDiscountRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDiscount>> GetByOrderIdAsync(int orderId) =>
            await _context.OrderDiscounts
                .Where(d => d.OrderId == orderId)
                .ToListAsync();

        public async Task AddAsync(OrderDiscount discount)
        {
            _context.OrderDiscounts.Add(discount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int orderDiscountId)
        {
            var entity = await _context.OrderDiscounts.FindAsync(orderDiscountId);
            if (entity is null) return;
            _context.OrderDiscounts.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
