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
    public class DiscountTargetService : IDiscountTargetService
    {
        private readonly IDiscountTargetRepository _repository;

        public DiscountTargetService(IDiscountTargetRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DiscountTargetDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(DiscountTargetMapper.ToDto).ToList();
        }

        public async Task<DiscountTargetDto?> GetByCodeAsync(string code)
        {
            var entity = await _repository.GetByCodeAsync(code);
            return entity is null ? null : DiscountTargetMapper.ToDto(entity);
        }

        public async Task CreateAsync(DiscountTargetDto dto)
        {
            var entity = DiscountTargetMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(DiscountTargetDto dto)
        {
            var entity = DiscountTargetMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string code) => await _repository.DeleteAsync(code);
    }
}
