using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Business.Service
{
    public class OrderDiscountService : IOrderDiscountService
    {
        private readonly IOrderDiscountRepository _repository;

        public OrderDiscountService(IOrderDiscountRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDiscountDto>> GetByOrderIdAsync(int orderId)
        {
            var entities = await _repository.GetByOrderIdAsync(orderId);
            return entities.Select(OrderDiscountMapper.ToDto).ToList();
        }

        public async Task AddAsync(OrderDiscountDto dto)
        {
            var entity = OrderDiscountMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int orderDiscountId) =>
            await _repository.DeleteAsync(orderDiscountId);
    }    
}
