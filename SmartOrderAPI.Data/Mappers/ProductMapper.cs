using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Product entity) => new ProductDto
        {
            ProductId = entity.ProductId,
            Name = entity.Name.Trim(),
            Description = entity.Description,
            CategoryId = entity.CategoryId,
            CategoryName = entity.Category?.Name ?? string.Empty,
            Sku = entity.Sku,
            SalePrice = entity.SalePrice,
            CurrentSalePrice = entity.SalePrice,
            HasCurrentPrice = entity.SalePrice > 0,
            IsActive = entity.IsActive,
            IsDirectSale = entity.IsDirectSale,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };

        public static Product ToEntity(this ProductDto dto) => new Product
        {
            ProductId = dto.ProductId,
            Name = dto.Name,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            Sku = dto.Sku,
            SalePrice = dto.SalePrice,
            IsActive = dto.IsActive,
            IsDirectSale = dto.IsDirectSale,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
