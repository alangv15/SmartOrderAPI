using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SmartOrderContext _context;

        public ProductRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .OrderByDescending(p => p.IsActive)
                .ThenBy(p => p.Category.Name)
                .ThenBy(p => p.Name)
                .Select(p => p.ToDto())
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.ProductId == id)
                .Select(p => p.ToDto())
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(ProductDto dto)
        {
            var entity = dto.ToEntity();
            if (entity.ProductId <= 0)
            {
                entity.ProductId = await GetNextProductIdAsync();
            }

            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;

            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            dto.ProductId = entity.ProductId;
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var existing = await _context.Products.FindAsync(dto.ProductId);
            if (existing != null)
            {
                existing.Name = dto.Name.Trim();
                existing.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
                existing.CategoryId = dto.CategoryId;
                existing.Sku = dto.Sku.Trim();
                existing.SalePrice = dto.SalePrice;
                existing.IsDirectSale = dto.IsDirectSale;
                existing.IsActive = dto.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsActive = false;
                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public Task<bool> CategoryExistsAsync(int categoryId)
        {
            return _context.Categories.AnyAsync(category => category.CategoryId == categoryId && category.IsActive);
        }

        public Task<bool> SkuExistsAsync(string sku, int? ignoreProductId = null)
        {
            var normalizedSku = sku.Trim();
            return _context.Products.AnyAsync(product =>
                product.Sku == normalizedSku &&
                (!ignoreProductId.HasValue || product.ProductId != ignoreProductId.Value));
        }

        private async Task<int> GetNextProductIdAsync()
        {
            var lastProductId = await _context.Products
                .Select(product => (int?)product.ProductId)
                .MaxAsync() ?? 0;

            return lastProductId + 1;
        }
    }
}
