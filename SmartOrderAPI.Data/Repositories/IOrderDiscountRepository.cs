using SmartOrderAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IOrderDiscountRepository
    {
        Task<List<OrderDiscount>> GetByOrderIdAsync(int orderId);
        Task AddAsync(OrderDiscount discount);
        Task DeleteAsync(int orderDiscountId);
    }    
}
