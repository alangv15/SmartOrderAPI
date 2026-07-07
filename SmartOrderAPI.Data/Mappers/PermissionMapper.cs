using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class PermissionMapper
    {
        public static PermissionDto ToDto(this Permission entity, bool isAssigned = false) => new PermissionDto
        {
            PermissionId = entity.PermissionId,
            Name = entity.Name,
            Code = entity.Code,
            Module = entity.Module,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            IsAssigned = isAssigned
        };
    }
}
