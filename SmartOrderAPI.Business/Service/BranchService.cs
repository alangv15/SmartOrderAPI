using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepo;

        public BranchService(IBranchRepository branchRepo)

        {
            _branchRepo = branchRepo;
        }

        public async Task<IEnumerable<BranchDto>> GetAllAsync()
        {
            return await _branchRepo.GetAllAsync();
        }

        public async Task<BranchDto?> GetByIdAsync(int id)
        {
            return await _branchRepo.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(BranchDto dto)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("El nombre de la sucursal es obligatorio.");

            await _branchRepo.AddAsync(dto);
            return dto.BranchId;
        }

        public async Task UpdateAsync(BranchDto dto)
        {
            if (dto.BranchId <= 0)
                throw new ArgumentException("ID inválido para actualizar.");

            await _branchRepo.UpdateAsync(dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _branchRepo.DeleteAsync(id);
        }
    }
}
