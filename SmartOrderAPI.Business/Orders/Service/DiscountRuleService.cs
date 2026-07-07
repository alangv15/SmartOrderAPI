using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Business.Orders.Service
{
    public class DiscountRuleService : IDiscountRuleService
    {
        private readonly IDiscountRuleRepository _repository;

        public DiscountRuleService(IDiscountRuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DiscountRuleDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(DiscountRuleMapper.ToDto).ToList();
        }

        public async Task<DiscountRuleDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : DiscountRuleMapper.ToDto(entity);
        }

        public async Task CreateAsync(DiscountRuleDto dto)
        {
            var entity = DiscountRuleMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(DiscountRuleDto dto)
        {
            var entity = DiscountRuleMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
