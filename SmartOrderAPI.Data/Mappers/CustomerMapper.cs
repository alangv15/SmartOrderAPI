using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class CustomerMapper
    {
        public static CustomerDto ToDto(this Customer entity) => new CustomerDto
        {
            CustomerId = entity.CustomerId,
            FullName = entity.FullName,
            Phone = entity.Phone,
            Email = entity.Email,
            Address = entity.Address,
            Neighborhood = entity.Neighborhood,
            City = entity.City,
            State = entity.State,
            PostalCode = entity.PostalCode,
            CreatedAt = entity.CreatedAt,
            IsActive = entity.IsActive
        };

        public static Customer ToEntity(this CustomerDto dto) => new Customer
        {
            CustomerId = dto.CustomerId,
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            Neighborhood = dto.Neighborhood,
            City = dto.City,
            State = dto.State,
            PostalCode = dto.PostalCode,
            CreatedAt = dto.CreatedAt,
            IsActive = dto.IsActive
        };
    }
}
