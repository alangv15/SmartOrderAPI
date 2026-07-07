using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(Category entity) => new()
        {
            CategoryId = entity.CategoryId,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            IsDirectSale = entity.IsDirectSale,
            DisplayOrder = entity.DisplayOrder
        };

        public static Category ToEntity(CategoryDto dto) => new()
        {
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive,
            IsDirectSale = dto.IsDirectSale,
            DisplayOrder = dto.DisplayOrder
        };
    }
}
