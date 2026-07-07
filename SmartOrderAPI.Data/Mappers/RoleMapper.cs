using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class RoleMapper
    {
        public static RoleDto ToDto(this Role entity) => new RoleDto
        {
            RoleId = entity.RoleId,
            Name = entity.Name,
            Code = entity.Code,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UserCount = entity.Users?.Count ?? 0
        };
    }
}
