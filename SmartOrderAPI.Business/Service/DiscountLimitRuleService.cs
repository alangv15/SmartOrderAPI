using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service;

public class DiscountLimitRuleService : IDiscountLimitRuleService
{
    private readonly IDiscountLimitRuleRepository _repository;

    public DiscountLimitRuleService(IDiscountLimitRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DiscountLimitRuleDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(DiscountLimitRuleMapper.ToDto).ToList();
    }

    public async Task<DiscountLimitRuleDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : DiscountLimitRuleMapper.ToDto(entity);
    }

    public async Task CreateAsync(DiscountLimitRuleDto dto)
    {
        var entity = DiscountLimitRuleMapper.ToEntity(dto);
        await _repository.AddAsync(entity);
        dto.DiscountLimitRuleId = entity.DiscountLimitRuleId;
    }

    public async Task UpdateAsync(DiscountLimitRuleDto dto)
    {
        var entity = DiscountLimitRuleMapper.ToEntity(dto);
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
}
