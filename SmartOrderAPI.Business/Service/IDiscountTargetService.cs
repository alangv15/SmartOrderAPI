using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Business.Service
{
    public interface IDiscountTargetService
    {
        Task<List<DiscountTargetDto>> GetAllAsync();
        Task<DiscountTargetDto?> GetByCodeAsync(string code);
        Task CreateAsync(DiscountTargetDto dto);
        Task UpdateAsync(DiscountTargetDto dto);
        Task DeleteAsync(string code);
    }    
}
