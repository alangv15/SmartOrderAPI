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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(CategoryMapper.ToDto).ToList();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : CategoryMapper.ToDto(entity);
        }

        public async Task CreateAsync(CategoryDto dto)
        {
            var entity = CategoryMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            dto.CategoryId = entity.CategoryId;
        }

        public async Task UpdateAsync(CategoryDto dto)
        {
            var entity = CategoryMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
