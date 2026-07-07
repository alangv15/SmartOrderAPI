using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class UserBranchMapper
    {
        public static UserBranchDto ToDto(this UserBranch entity) => new UserBranchDto
        {
            UserBranchId = entity.UserBranchId,
            UserId = entity.UserId,
            BranchId = entity.BranchId,
            AssignedAt = entity.AssignedAt
        };

        public static UserBranch ToEntity(this UserBranchDto dto) => new UserBranch
        {
            UserBranchId = dto.UserBranchId,
            UserId = dto.UserId,
            BranchId = dto.BranchId,
            AssignedAt = dto.AssignedAt
        };
    }
}
