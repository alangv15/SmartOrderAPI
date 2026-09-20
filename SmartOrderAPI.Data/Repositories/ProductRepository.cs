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
            var effectiveDate = DateTime.UtcNow.Date;

            var products = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .OrderByDescending(p => p.IsActive)
                .ThenBy(p => p.Category.Name)
                .ThenBy(p => p.Name)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name.Trim(),
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Sku = p.Sku,
                    SalePrice = p.SalePrice,
                    CurrentProductPriceId = _context.ProductPrices
                        .Where(price => price.ProductId == p.ProductId)
                        .Where(price => price.EffectiveFrom <= effectiveDate)
                        .Where(price => price.EffectiveTo == null || price.EffectiveTo >= effectiveDate)
                        .OrderByDescending(price => price.EffectiveFrom)
                        .Select(price => (int?)price.ProductPriceId)
                        .FirstOrDefault(),
                    CurrentSalePrice = _context.ProductPrices
                        .Where(price => price.ProductId == p.ProductId)
                        .Where(price => price.EffectiveFrom <= effectiveDate)
                        .Where(price => price.EffectiveTo == null || price.EffectiveTo >= effectiveDate)
                        .OrderByDescending(price => price.EffectiveFrom)
                        .Select(price => (decimal?)price.SalePrice)
                        .FirstOrDefault(),
                    CurrentProductRecipeId = _context.ProductRecipes
                        .Where(recipe => recipe.ProductId == p.ProductId)
                        .Where(recipe => recipe.IsActive)
                        .Where(recipe => recipe.EffectiveFrom <= effectiveDate)
                        .Where(recipe => recipe.EffectiveTo == null || recipe.EffectiveTo >= effectiveDate)
                        .OrderByDescending(recipe => recipe.EffectiveFrom)
                        .Select(recipe => (int?)recipe.ProductRecipeId)
                        .FirstOrDefault(),
                    CurrentUnitCost = _context.ProductRecipes
                        .Where(recipe => recipe.ProductId == p.ProductId)
                        .Where(recipe => recipe.IsActive)
                        .Where(recipe => recipe.EffectiveFrom <= effectiveDate)
                        .Where(recipe => recipe.EffectiveTo == null || recipe.EffectiveTo >= effectiveDate)
                        .OrderByDescending(recipe => recipe.EffectiveFrom)
                        .Select(recipe => (decimal?)recipe.RecipeCost)
                        .FirstOrDefault(),
                    IsActive = p.IsActive,
                    IsDirectSale = p.IsDirectSale,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            ApplyCurrentFlags(products);
            return products;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var effectiveDate = DateTime.UtcNow.Date;

            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name.Trim(),
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Sku = p.Sku,
                    SalePrice = p.SalePrice,
                    CurrentProductPriceId = _context.ProductPrices
                        .Where(price => price.ProductId == p.ProductId)
                        .Where(price => price.EffectiveFrom <= effectiveDate)
                        .Where(price => price.EffectiveTo == null || price.EffectiveTo >= effectiveDate)
                        .OrderByDescending(price => price.EffectiveFrom)
                        .Select(price => (int?)price.ProductPriceId)
                        .FirstOrDefault(),
                    CurrentSalePrice = _context.ProductPrices
                        .Where(price => price.ProductId == p.ProductId)
                        .Where(price => price.EffectiveFrom <= effectiveDate)
                        .Where(price => price.EffectiveTo == null || price.EffectiveTo >= effectiveDate)
                        .OrderByDescending(price => price.EffectiveFrom)
                        .Select(price => (decimal?)price.SalePrice)
                        .FirstOrDefault(),
                    CurrentProductRecipeId = _context.ProductRecipes
                        .Where(recipe => recipe.ProductId == p.ProductId)
                        .Where(recipe => recipe.IsActive)
                        .Where(recipe => recipe.EffectiveFrom <= effectiveDate)
                        .Where(recipe => recipe.EffectiveTo == null || recipe.EffectiveTo >= effectiveDate)
                        .OrderByDescending(recipe => recipe.EffectiveFrom)
                        .Select(recipe => (int?)recipe.ProductRecipeId)
                        .FirstOrDefault(),
                    CurrentUnitCost = _context.ProductRecipes
                        .Where(recipe => recipe.ProductId == p.ProductId)
                        .Where(recipe => recipe.IsActive)
                        .Where(recipe => recipe.EffectiveFrom <= effectiveDate)
                        .Where(recipe => recipe.EffectiveTo == null || recipe.EffectiveTo >= effectiveDate)
                        .OrderByDescending(recipe => recipe.EffectiveFrom)
                        .Select(recipe => (decimal?)recipe.RecipeCost)
                        .FirstOrDefault(),
                    IsActive = p.IsActive,
                    IsDirectSale = p.IsDirectSale,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (product != null)
            {
                ApplyCurrentFlags(new[] { product });
            }

            return product;
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

            _context.ProductPrices.Add(new SmartOrderAPI.Data.Models.ProductPrice
            {
                ProductId = entity.ProductId,
                SalePrice = entity.SalePrice,
                EffectiveFrom = DateTime.UtcNow.Date,
                CreatedAt = DateTime.UtcNow,
                Notes = "Initial price loaded from product creation"
            });
            await _context.SaveChangesAsync();

            dto.ProductId = entity.ProductId;
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var existing = await _context.Products.FindAsync(dto.ProductId);
            if (existing != null)
            {
                var previousSalePrice = existing.SalePrice;

                existing.Name = dto.Name.Trim();
                existing.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
                existing.CategoryId = dto.CategoryId;
                existing.Sku = dto.Sku.Trim();
                existing.SalePrice = dto.SalePrice;
                existing.IsDirectSale = dto.IsDirectSale;
                existing.IsActive = dto.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;

                if (previousSalePrice != dto.SalePrice)
                {
                    await UpdateProductPriceHistoryAsync(existing.ProductId, dto.SalePrice);
                }

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

        private static void ApplyCurrentFlags(IEnumerable<ProductDto> products)
        {
            foreach (var product in products)
            {
                product.HasCurrentPrice = product.CurrentProductPriceId.HasValue;
                product.HasCurrentCost = product.CurrentProductRecipeId.HasValue && (product.CurrentUnitCost ?? 0) > 0;
            }
        }

        private async Task UpdateProductPriceHistoryAsync(int productId, decimal newSalePrice)
        {
            var today = DateTime.UtcNow.Date;
            var now = DateTime.UtcNow;
            var currentPrice = await _context.ProductPrices
                .Where(price => price.ProductId == productId)
                .Where(price => price.EffectiveTo == null)
                .OrderByDescending(price => price.EffectiveFrom)
                .FirstOrDefaultAsync();

            if (currentPrice == null)
            {
                _context.ProductPrices.Add(new SmartOrderAPI.Data.Models.ProductPrice
                {
                    ProductId = productId,
                    SalePrice = newSalePrice,
                    EffectiveFrom = today,
                    CreatedAt = now,
                    Notes = "Initial price loaded from product edit"
                });
                return;
            }

            if (currentPrice.EffectiveFrom.Date == today)
            {
                currentPrice.SalePrice = newSalePrice;
                currentPrice.Notes = string.IsNullOrWhiteSpace(currentPrice.Notes)
                    ? "Price updated from product edit"
                    : currentPrice.Notes;
                return;
            }

            currentPrice.EffectiveTo = today.AddDays(-1);
            _context.ProductPrices.Add(new SmartOrderAPI.Data.Models.ProductPrice
            {
                ProductId = productId,
                SalePrice = newSalePrice,
                EffectiveFrom = today,
                CreatedAt = now,
                Notes = "Price updated from product edit"
            });
        }
    }
}
