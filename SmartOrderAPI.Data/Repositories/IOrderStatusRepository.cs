using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IOrderStatusRepository
    {
        Task<IEnumerable<OrderStatusDto>> GetAllAsync();
        Task<OrderStatusDto?> GetByCodeAsync(string code);
        Task AddAsync(OrderStatusDto dto);
        Task UpdateAsync(OrderStatusDto dto);
        Task DeleteAsync(string code);
    }
}
