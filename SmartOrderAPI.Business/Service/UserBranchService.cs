using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class UserBranchService : IUserBranchService
    {
        private readonly IUserBranchRepository _userBranchRepo;

        public UserBranchService(IUserBranchRepository userBranchRepo)
        {
            _userBranchRepo = userBranchRepo;
        }

        public async Task<IEnumerable<UserBranchDto>> GetByUserIdAsync(int userId)
        {
            return await _userBranchRepo.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<UserBranchDto>> GetByBranchIdAsync(int branchId)
        {
            return await _userBranchRepo.GetByBranchIdAsync(branchId);
        }

        public async Task<UserBranchDto?> GetByIdAsync(int id)
        {
            return await _userBranchRepo.GetByIdAsync(id);
        }

        public async Task<int> AssignAsync(UserBranchDto dto)
        {
            // Validación de negocio: evitar duplicados
            var existing = await _userBranchRepo.GetByUserIdAsync(dto.UserId);
            if (existing.Any(ub => ub.BranchId == dto.BranchId))
                throw new InvalidOperationException("El usuario ya está asignado a esta sucursal.");

            await _userBranchRepo.AddAsync(dto);
            return dto.UserBranchId;
        }

        public async Task UnassignAsync(int id)
        {
            await _userBranchRepo.DeleteAsync(id);
        }
    }
}
