using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SmartOrderContext _context;

        public CustomerRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            return await _context.Customers
                .Where(c => c.IsActive)
                .Select(c => c.ToDto())
                .ToListAsync();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && c.IsActive);

            return customer?.ToDto();
        }

        public async Task AddAsync(CustomerDto customerDto)
        {
            var entity = customerDto.ToEntity();
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;

            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();

            customerDto.CustomerId = entity.CustomerId;
        }

        public async Task UpdateAsync(CustomerDto customerDto)
        {
            var existing = await _context.Customers.FindAsync(customerDto.CustomerId);
            if (existing != null && existing.IsActive)
            {
                var updated = customerDto.ToEntity();
                _context.Entry(existing).CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                customer.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
