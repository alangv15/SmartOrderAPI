using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class DiscountTypeService : IDiscountTypeService
    {
        private readonly IDiscountTypeRepository _discountTypeRepo;

        public DiscountTypeService(IDiscountTypeRepository discountTypeRepo)
        {
            _discountTypeRepo = discountTypeRepo;
        }

        public async Task<IEnumerable<DiscountTypeDto>> GetAllAsync()
        {
            return await _discountTypeRepo.GetAllAsync();
        }

        public async Task<DiscountTypeDto?> GetByCodeAsync(string code)
        {
            return await _discountTypeRepo.GetByCodeAsync(code);
        }

        public async Task CreateAsync(DiscountTypeDto dto)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(dto.DiscountTypeCode))
                throw new ArgumentException("El código del tipo de descuento es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.DisplayName))
                throw new ArgumentException("El nombre del tipo de descuento es obligatorio.");

            await _discountTypeRepo.AddAsync(dto);
        }

        public async Task UpdateAsync(DiscountTypeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DiscountTypeCode))
                throw new ArgumentException("Código inválido para actualizar.");

            await _discountTypeRepo.UpdateAsync(dto);
        }

        public async Task DeleteAsync(string code)
        {
            await _discountTypeRepo.DeleteAsync(code);
        }
    }
}
