using SmartOrderAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Repositories
{
    public interface IDiscountTargetRepository
    {
        Task<List<DiscountTarget>> GetAllAsync();
        Task<DiscountTarget?> GetByCodeAsync(string code);
        Task AddAsync(DiscountTarget target);
        Task UpdateAsync(DiscountTarget target);
        Task DeleteAsync(string code);
    }
}
