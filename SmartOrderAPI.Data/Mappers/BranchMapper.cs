using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class BranchMapper
    {
        public static BranchDto ToDto(this Branch entity) => new BranchDto
        {
            BranchId = entity.BranchId,
            Name = entity.Name,
            Code = entity.Code,
            Address = entity.Address,
            City = entity.City,
            State = entity.State,
            PostalCode = entity.PostalCode,
            Phone = entity.Phone,
            Email = entity.Email,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };

        public static Branch ToEntity(this BranchDto dto) => new Branch
        {
            BranchId = dto.BranchId,
            Name = dto.Name,
            Code = dto.Code,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            PostalCode = dto.PostalCode,
            Phone = dto.Phone,
            Email = dto.Email,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt
        };
    }

}
