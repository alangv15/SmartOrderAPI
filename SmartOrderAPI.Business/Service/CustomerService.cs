using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            return await _customerRepo.GetAllAsync();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            return await _customerRepo.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(CustomerDto dto)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("El nombre del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("El correo electrónico es obligatorio.");

            await _customerRepo.AddAsync(dto);
            return dto.CustomerId;
        }

        public async Task UpdateAsync(CustomerDto dto)
        {
            if (dto.CustomerId <= 0)
                throw new ArgumentException("ID inválido para actualizar.");

            await _customerRepo.UpdateAsync(dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _customerRepo.DeleteAsync(id);
        }
    }
}
