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
    public class DiscountRuleTargetService : IDiscountRuleTargetService
    {
        private readonly IDiscountRuleTargetRepository _repository;

        public DiscountRuleTargetService(IDiscountRuleTargetRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DiscountRuleTargetDto>> GetByRuleIdAsync(int discountRuleId)
        {
            var entities = await _repository.GetByRuleIdAsync(discountRuleId);
            return entities.Select(DiscountRuleTargetMapper.ToDto).ToList();
        }

        public async Task AddAsync(DiscountRuleTargetDto dto)
        {
            var entity = DiscountRuleTargetMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int discountRuleTargetId) =>
            await _repository.DeleteAsync(discountRuleTargetId);
    }
}
