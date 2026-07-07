using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Mappers
{
    public static class PaymentStatusMapper
    {
        public static PaymentStatusDto ToDto(this PaymentStatus entity) => new PaymentStatusDto
        {
            PaymentStatusCode = entity.PaymentStatusCode,
            DisplayName = entity.DisplayName,
            IsActive = entity.IsActive
        };

        public static PaymentStatus ToEntity(this PaymentStatusDto dto) => new PaymentStatus
        {
            PaymentStatusCode = dto.PaymentStatusCode,
            DisplayName = dto.DisplayName,
            IsActive = dto.IsActive
        };
    }
}
