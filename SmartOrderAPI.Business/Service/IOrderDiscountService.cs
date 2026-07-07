using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Business.Service
{
    public interface IOrderDiscountService
    {
        Task<List<OrderDiscountDto>> GetByOrderIdAsync(int orderId);
        Task AddAsync(OrderDiscountDto dto);
        Task DeleteAsync(int orderDiscountId);
    }
}
