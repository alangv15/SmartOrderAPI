using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await _productRepo.GetAllAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _productRepo.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(ProductDto dto)
        {
            await ValidateProductAsync(dto, null);

            dto.Name = dto.Name.Trim();
            dto.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            dto.Sku = dto.Sku.Trim();
            dto.CreatedAt = DateTime.UtcNow;
            dto.IsActive = true;

            await _productRepo.AddAsync(dto);
            return dto.ProductId;
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            if (dto.ProductId <= 0)
                throw new ArgumentException("ID inválido para actualizar.");

            await ValidateProductAsync(dto, dto.ProductId);
            dto.Name = dto.Name.Trim();
            dto.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            dto.Sku = dto.Sku.Trim();
            dto.UpdatedAt = DateTime.UtcNow;
            await _productRepo.UpdateAsync(dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepo.DeleteAsync(id);
        }

        private async Task ValidateProductAsync(ProductDto dto, int? ignoreProductId)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("El nombre del producto es obligatorio.");

            if (dto.CategoryId <= 0)
                throw new ArgumentException("La categoria es obligatoria.");

            if (!await _productRepo.CategoryExistsAsync(dto.CategoryId))
                throw new ArgumentException("La categoria seleccionada no existe o esta inactiva.");

            if (string.IsNullOrWhiteSpace(dto.Sku))
                throw new ArgumentException("El SKU es obligatorio.");

            if (await _productRepo.SkuExistsAsync(dto.Sku, ignoreProductId))
                throw new ArgumentException("Ya existe un producto con ese SKU.");

            if (dto.SalePrice <= 0)
                throw new ArgumentException("El precio de venta debe ser mayor a cero.");
        }
    }
}
