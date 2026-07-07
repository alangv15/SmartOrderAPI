using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User entity) => new UserDto
        {
            UserId = entity.UserId,
            FullName = entity.FullName,
            Email = entity.Email,
            RoleId = entity.RoleId,
            RoleCode = entity.Role?.Code ?? string.Empty,
            RoleName = entity.Role?.Name ?? string.Empty,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            LastLogin = entity.LastLogin
        };

        public static User ToEntity(this UserDto dto) => new User
        {
            UserId = dto.UserId,
            FullName = dto.FullName,
            Email = dto.Email,
            RoleId = dto.RoleId,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt,
            LastLogin = dto.LastLogin
        };
    }
}
