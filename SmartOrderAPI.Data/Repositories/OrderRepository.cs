using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private const string CancelledOrderStatusCode = "Cancelled";
        private const string CompletedOrderStatusCode = "Completed";
        private const string InStoreSalesChannelCode = "In-Store";
        private const string CustomOrderSalesChannelCode = "Order";

        private readonly SmartOrderContext _context;

        public OrderRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            return await _context.Orders
                .Where(o => o.OrderStatusCode != CancelledOrderStatusCode)
                .Select(o => o.ToDto())
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetActiveCustomOrdersAsync()
        {
            return await _context.Orders
                .Where(o => o.OrderStatusCode != CancelledOrderStatusCode)
                .Where(o => o.OrderStatusCode != CompletedOrderStatusCode)
                .Where(o => o.SalesChannel == CustomOrderSalesChannelCode)
                .Where(o => !o.IsDirectSale)
                .Select(o => o.ToDto())
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetCustomOrderSummariesAsync(DateTime startUtc, DateTime endExclusiveUtc)
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(item => item.Product)
                .Where(o => o.OrderStatusCode != CancelledOrderStatusCode)
                .Where(o => o.SalesChannel == CustomOrderSalesChannelCode)
                .Where(o => !o.IsDirectSale)
                .Where(o => o.DeliveryDate.HasValue)
                .Where(o => o.DeliveryDate!.Value >= startUtc && o.DeliveryDate.Value < endExclusiveUtc)
                .OrderBy(o => o.DeliveryDate)
                .ThenBy(o => o.OrderId)
                .ToListAsync();

            return orders.Select(order => new OrderSummaryDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.FullName?.Trim() ?? "Cliente no encontrado",
                Pieces = order.Pieces,
                TotalAmount = order.TotalAmount,
                ProductionStartDate = order.ProductionStartDate,
                ProductionEndDate = order.ProductionEndDate,
                DeliveryDate = order.DeliveryDate,
                OrderStatusCode = order.OrderStatusCode,
                PaymentStatusCode = order.PaymentStatusCode,
                IsInternalProduction = order.IsInternalProduction,
                ProductSummary = BuildProductSummary(order.OrderItems),
                Comments = order.Comments
            }).ToList();
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderDiscounts)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.OrderStatusCode != CancelledOrderStatusCode);

            return order?.ToDto();
        }

        public async Task<OrderDto?> GetInStoreSaleByIdAsync(int id)
        {
            var order = await GetOrderWithDetailsQuery()
                .FirstOrDefaultAsync(o =>
                    o.OrderId == id &&
                    o.OrderStatusCode != CancelledOrderStatusCode &&
                    o.SalesChannel == InStoreSalesChannelCode &&
                    o.IsDirectSale);

            return order?.ToDto();
        }

        public async Task<OrderDto?> GetCustomOrderByIdAsync(int id)
        {
            var order = await GetOrderWithDetailsQuery()
                .FirstOrDefaultAsync(o =>
                    o.OrderId == id &&
                    o.SalesChannel == CustomOrderSalesChannelCode &&
                    !o.IsDirectSale);

            return order?.ToDto();
        }

        public async Task AddAsync(OrderDto orderDto)
        {
            var entity = orderDto.ToEntity();

            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();

            orderDto.OrderId = entity.OrderId;
        }

        public async Task UpdateAsync(OrderDto orderDto)
        {
            var existing = await _context.Orders
                .Include(order => order.OrderItems)
                .Include(order => order.OrderDiscounts)
                .FirstOrDefaultAsync(order => order.OrderId == orderDto.OrderId);

            if (existing == null)
            {
                throw new InvalidOperationException("Pedido no encontrado.");
            }

            existing.BranchId = orderDto.BranchId;
            existing.UserId = orderDto.UserId;
            existing.CustomerId = orderDto.CustomerId;
            existing.Pieces = orderDto.Pieces;
            existing.DiscountAmount = orderDto.DiscountAmount;
            existing.TotalAmount = orderDto.TotalAmount;
            existing.CashReceivedAmount = orderDto.CashReceivedAmount;
            existing.CashChangeAmount = orderDto.CashChangeAmount;
            existing.ProductionStartDate = orderDto.ProductionStartDate;
            existing.ProductionEndDate = orderDto.ProductionEndDate;
            existing.DeliveryDate = orderDto.DeliveryDate;
            existing.OrderStatusCode = orderDto.OrderStatusCode;
            existing.PaymentStatusCode = orderDto.PaymentStatusCode;
            existing.PaymentMethod = orderDto.PaymentMethod;
            existing.SalesChannel = orderDto.SalesChannel;
            existing.Comments = orderDto.Comments;
            existing.CustomerGender = orderDto.CustomerGender;
            existing.CustomerAgeRange = orderDto.CustomerAgeRange;
            existing.CustomerType = orderDto.CustomerType;
            existing.AcquisitionChannel = orderDto.AcquisitionChannel;
            existing.IsDirectSale = orderDto.IsDirectSale;
            existing.IsInternalProduction = orderDto.IsInternalProduction;
            existing.UpdatedAt = DateTime.UtcNow;

            if (existing.OrderItems.Any())
            {
                _context.OrderItems.RemoveRange(existing.OrderItems);
                existing.OrderItems.Clear();
            }

            if (existing.OrderDiscounts.Any())
            {
                _context.OrderDiscounts.RemoveRange(existing.OrderDiscounts);
                existing.OrderDiscounts.Clear();
            }

            foreach (var itemDto in orderDto.OrderItems)
            {
                var orderItem = itemDto.ToEntity();
                orderItem.OrderId = existing.OrderId;
                existing.OrderItems.Add(orderItem);
            }

            foreach (var discountDto in orderDto.OrderDiscounts)
            {
                var orderDiscount = OrderDiscountMapper.ToEntity(discountDto);
                orderDiscount.OrderId = existing.OrderId;
                existing.OrderDiscounts.Add(orderDiscount);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, OrderStatusUpdateDto dto)
        {
            var existing = await _context.Orders
                .FirstOrDefaultAsync(order =>
                    order.OrderId == id &&
                    order.SalesChannel == CustomOrderSalesChannelCode &&
                    !order.IsDirectSale);

            if (existing == null)
            {
                throw new InvalidOperationException("Pedido no encontrado.");
            }

            existing.OrderStatusCode = dto.OrderStatusCode.Trim();
            existing.PaymentStatusCode = dto.PaymentStatusCode.Trim();
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatusCode = "Cancelled";
                order.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        private IQueryable<SmartOrderAPI.Data.Models.Order> GetOrderWithDetailsQuery()
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderDiscounts);
        }

        private static string BuildProductSummary(IEnumerable<SmartOrderAPI.Data.Models.OrderItem> items)
        {
            var productParts = items
                .OrderBy(item => item.Product.Name)
                .Select(item => $"{item.Quantity} {item.Product.Name}".Trim())
                .ToList();

            return productParts.Count == 0
                ? "Sin productos"
                : string.Join(", ", productParts);
        }
    }
}
