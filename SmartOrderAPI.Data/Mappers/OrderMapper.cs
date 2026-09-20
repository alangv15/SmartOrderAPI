using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Data.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order entity) => new OrderDto
        {
            OrderId = entity.OrderId,
            BranchId = entity.BranchId,
            UserId = entity.UserId,
            CustomerId = entity.CustomerId,
            Pieces = entity.Pieces,
            DiscountAmount = entity.DiscountAmount,
            TotalAmount = entity.TotalAmount,
            CashReceivedAmount = entity.CashReceivedAmount,
            CashChangeAmount = entity.CashChangeAmount,
            ProductionStartDate = entity.ProductionStartDate,
            ProductionEndDate = entity.ProductionEndDate,
            DeliveryDate = entity.DeliveryDate,
            OrderStatusCode = entity.OrderStatusCode,
            PaymentStatusCode = entity.PaymentStatusCode,
            PaymentMethod = entity.PaymentMethod,
            SalesChannel = entity.SalesChannel,
            Comments = entity.Comments,
            CustomerGender = entity.CustomerGender,
            CustomerAgeRange = entity.CustomerAgeRange,
            CustomerType = entity.CustomerType,
            AcquisitionChannel = entity.AcquisitionChannel,
            IsDirectSale = entity.IsDirectSale,
            IsInternalProduction = entity.IsInternalProduction,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            OrderDiscounts = entity.OrderDiscounts.Select(od => OrderDiscountMapper.ToDto(od)).ToList(),
            OrderItems = entity.OrderItems.Select(it => OrderItemMapper.ToDto(it)).ToList()
        };

        public static Order ToEntity(this OrderDto dto) => new Order
        {
            OrderId = dto.OrderId,
            BranchId = dto.BranchId,
            UserId = dto.UserId,
            CustomerId = dto.CustomerId,
            Pieces = dto.Pieces,
            DiscountAmount = dto.DiscountAmount,
            TotalAmount = dto.TotalAmount,
            CashReceivedAmount = dto.CashReceivedAmount,
            CashChangeAmount = dto.CashChangeAmount,
            ProductionStartDate = dto.ProductionStartDate,
            ProductionEndDate = dto.ProductionEndDate,
            DeliveryDate = dto.DeliveryDate,
            OrderStatusCode = dto.OrderStatusCode,
            PaymentStatusCode = dto.PaymentStatusCode,
            PaymentMethod = dto.PaymentMethod,
            SalesChannel = dto.SalesChannel,
            Comments = dto.Comments,
            CustomerGender = dto.CustomerGender,
            CustomerAgeRange = dto.CustomerAgeRange,
            CustomerType = dto.CustomerType,
            AcquisitionChannel = dto.AcquisitionChannel,
            IsDirectSale = dto.IsDirectSale,
            IsInternalProduction = dto.IsInternalProduction,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            OrderDiscounts = dto.OrderDiscounts.Select(od => OrderDiscountMapper.ToEntity(od)).ToList(),
            OrderItems = dto.OrderItems.Select(it => OrderItemMapper.ToEntity(it)).ToList()
        };
    }
}
