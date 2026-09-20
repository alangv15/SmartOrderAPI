using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartOrderAPI.Data.Configuration;
using SmartOrderAPI.Entities.Reports.Models;

namespace SmartOrderAPI.Data.Reports.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private const string CancelledOrderStatusCode = "Cancelled";
        private const string InStoreSalesChannelCode = "In-Store";
        private const string CashPaymentMethodCode = "Cash";
        private const string CardPaymentMethodCode = "Card";

        private readonly SmartOrderContext _context;
        private readonly TimeZoneInfo _businessTimeZone;

        public ReportRepository(SmartOrderContext context, IOptions<BusinessTimeOptions> businessTimeOptions)
        {
            _context = context;
            _businessTimeZone = ResolveTimeZone(businessTimeOptions.Value.TimeZoneId);
        }

        public async Task<DailySalesReportDto> GetDailySalesReportAsync(DateTime? date)
        {
            var localDate = date?.Date ?? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _businessTimeZone).Date;
            var startOfDayLocal = DateTime.SpecifyKind(localDate, DateTimeKind.Unspecified);
            var endOfDayLocal = startOfDayLocal.AddDays(1);
            var startOfDayUtc = TimeZoneInfo.ConvertTimeToUtc(startOfDayLocal, _businessTimeZone);
            var endOfDayUtc = TimeZoneInfo.ConvertTimeToUtc(endOfDayLocal, _businessTimeZone);

            var items = await _context.OrderItems
                .AsNoTracking()
                .Where(oi =>
                    oi.Order.OrderStatusCode != CancelledOrderStatusCode &&
                    oi.Order.SalesChannel == InStoreSalesChannelCode &&
                    oi.Order.IsDirectSale &&
                    oi.Order.CreatedAt >= startOfDayUtc &&
                    oi.Order.CreatedAt < endOfDayUtc)
                .OrderBy(oi => oi.Order.CreatedAt)
                .Select(oi => new
                {
                    Folio = oi.Order.OrderId,
                    Category = oi.Product.Category.Name,
                    Product = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Total = oi.LineTotal ?? ((oi.Quantity * oi.UnitPrice) - (oi.DiscountAmount ?? 0)),
                    PaymentMethod = oi.Order.PaymentMethod,
                    TimeUtc = oi.Order.CreatedAt
                })
                .ToListAsync();

            return new DailySalesReportDto
            {
                Date = localDate,
                TotalSalesAmount = items.Sum(i => i.Total),
                TotalItemsSold = items.Sum(i => i.Quantity),
                Items = items.Select(i => new DailySalesReportItemDto
                {
                    Folio = i.Folio,
                    Category = i.Category,
                    Product = i.Product,
                    Quantity = i.Quantity,
                    Total = i.Total,
                    PaymentMethod = i.PaymentMethod,
                    Time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(i.TimeUtc, DateTimeKind.Utc), _businessTimeZone)
                }).ToList()
            };
        }

        public async Task<SalesSummaryReportDto> GetSalesSummaryReportAsync(DateTime? startDate, DateTime? endDate)
        {
            var todayLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _businessTimeZone).Date;
            var localStartDate = startDate?.Date ?? todayLocal;
            var localEndDate = endDate?.Date ?? localStartDate;

            if (localEndDate < localStartDate)
            {
                (localStartDate, localEndDate) = (localEndDate, localStartDate);
            }

            var startLocal = DateTime.SpecifyKind(localStartDate, DateTimeKind.Unspecified);
            var endExclusiveLocal = DateTime.SpecifyKind(localEndDate.AddDays(1), DateTimeKind.Unspecified);
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, _businessTimeZone);
            var endExclusiveUtc = TimeZoneInfo.ConvertTimeToUtc(endExclusiveLocal, _businessTimeZone);

            var orders = await _context.Orders
                .AsNoTracking()
                .Where(order =>
                    order.OrderStatusCode != CancelledOrderStatusCode &&
                    !order.IsInternalProduction &&
                    order.CreatedAt >= startUtc &&
                    order.CreatedAt < endExclusiveUtc)
                .Select(order => new
                {
                    order.CreatedAt,
                    order.PaymentMethod,
                    order.TotalAmount,
                    order.Pieces
                })
                .ToListAsync();

            var dailyTotals = orders
                .Select(order => new
                {
                    Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(order.CreatedAt, DateTimeKind.Utc), _businessTimeZone).Date,
                    order.PaymentMethod,
                    order.TotalAmount,
                    order.Pieces
                })
                .GroupBy(order => order.Date)
                .ToDictionary(
                    group => group.Key,
                    group => new SalesSummaryReportDayDto
                    {
                        Date = group.Key,
                        TotalSalesAmount = group.Sum(order => order.TotalAmount),
                        CashSalesAmount = group
                            .Where(order => order.PaymentMethod == CashPaymentMethodCode)
                            .Sum(order => order.TotalAmount),
                        CardSalesAmount = group
                            .Where(order => order.PaymentMethod == CardPaymentMethodCode)
                            .Sum(order => order.TotalAmount),
                        TotalItemsSold = group.Sum(order => order.Pieces)
                    });

            var days = Enumerable.Range(0, (localEndDate - localStartDate).Days + 1)
                .Select(offset => localStartDate.AddDays(offset))
                .Select(date => dailyTotals.TryGetValue(date, out var total)
                    ? total
                    : new SalesSummaryReportDayDto { Date = date })
                .OrderBy(day => day.Date)
                .ToList();

            return new SalesSummaryReportDto
            {
                StartDate = localStartDate,
                EndDate = localEndDate,
                TotalSalesAmount = days.Sum(day => day.TotalSalesAmount),
                CashSalesAmount = days.Sum(day => day.CashSalesAmount),
                CardSalesAmount = days.Sum(day => day.CardSalesAmount),
                TotalItemsSold = days.Sum(day => day.TotalItemsSold),
                Days = days
            };
        }

        public async Task<MonthlyProfitReportDto> GetMonthlyProfitReportAsync(DateTime? endMonth, int months)
        {
            var todayLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _businessTimeZone).Date;
            var requestedEndMonth = endMonth?.Date ?? todayLocal;
            var localEndMonth = new DateTime(requestedEndMonth.Year, requestedEndMonth.Month, 1);
            var localStartMonth = localEndMonth.AddMonths(-(months - 1));
            var endExclusiveMonth = localEndMonth.AddMonths(1);

            var startUtc = TimeZoneInfo.ConvertTimeToUtc(
                DateTime.SpecifyKind(localStartMonth, DateTimeKind.Unspecified),
                _businessTimeZone);
            var endExclusiveUtc = TimeZoneInfo.ConvertTimeToUtc(
                DateTime.SpecifyKind(endExclusiveMonth, DateTimeKind.Unspecified),
                _businessTimeZone);

            var orders = await _context.Orders
                .AsNoTracking()
                .Where(order =>
                    order.OrderStatusCode != CancelledOrderStatusCode &&
                    !order.IsInternalProduction &&
                    order.CreatedAt >= startUtc &&
                    order.CreatedAt < endExclusiveUtc)
                .Select(order => new
                {
                    order.CreatedAt,
                    order.PaymentMethod,
                    Revenue = order.TotalAmount,
                    Cost = order.OrderItems.Sum(item => (decimal?)(item.Quantity * item.UnitCost)) ?? 0m
                })
                .ToListAsync();

            var totalsByMonth = orders
                .Select(order => new
                {
                    Month = ToLocalMonth(order.CreatedAt),
                    order.PaymentMethod,
                    order.Revenue,
                    order.Cost
                })
                .GroupBy(order => order.Month)
                .ToDictionary(
                    group => group.Key,
                    group => new
                    {
                        Revenue = group.Sum(order => order.Revenue),
                        Cash = group
                            .Where(order => order.PaymentMethod == CashPaymentMethodCode)
                            .Sum(order => order.Revenue),
                        Card = group
                            .Where(order => order.PaymentMethod == CardPaymentMethodCode)
                            .Sum(order => order.Revenue),
                        Cost = group.Sum(order => order.Cost)
                    });

            var monthRows = new List<MonthlyProfitReportMonthDto>(months);
            for (var offset = 0; offset < months; offset++)
            {
                var month = localStartMonth.AddMonths(offset);
                totalsByMonth.TryGetValue(month, out var totals);

                var revenue = totals?.Revenue ?? 0m;
                var cost = totals?.Cost ?? 0m;
                var grossProfit = revenue - cost;

                monthRows.Add(new MonthlyProfitReportMonthDto
                {
                    Month = month,
                    TotalRevenueAmount = revenue,
                    CashRevenueAmount = totals?.Cash ?? 0m,
                    CardRevenueAmount = totals?.Card ?? 0m,
                    TotalCostAmount = cost,
                    GrossProfitAmount = grossProfit,
                    GrossMarginPercentage = CalculateMargin(revenue, grossProfit)
                });
            }

            var totalRevenue = monthRows.Sum(month => month.TotalRevenueAmount);
            var totalCost = monthRows.Sum(month => month.TotalCostAmount);
            var totalGrossProfit = totalRevenue - totalCost;

            return new MonthlyProfitReportDto
            {
                StartMonth = localStartMonth,
                EndMonth = localEndMonth,
                TotalRevenueAmount = totalRevenue,
                CashRevenueAmount = monthRows.Sum(month => month.CashRevenueAmount),
                CardRevenueAmount = monthRows.Sum(month => month.CardRevenueAmount),
                TotalCostAmount = totalCost,
                GrossProfitAmount = totalGrossProfit,
                GrossMarginPercentage = CalculateMargin(totalRevenue, totalGrossProfit),
                Months = monthRows
            };
        }

        private DateTime ToLocalMonth(DateTime utcDate)
        {
            var localDate = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(utcDate, DateTimeKind.Utc),
                _businessTimeZone);
            return new DateTime(localDate.Year, localDate.Month, 1);
        }

        private static decimal CalculateMargin(decimal revenue, decimal grossProfit)
        {
            return revenue == 0
                ? 0
                : decimal.Round((grossProfit / revenue) * 100m, 2, MidpointRounding.AwayFromZero);
        }

        private static TimeZoneInfo ResolveTimeZone(string? timeZoneId)
        {
            if (string.IsNullOrWhiteSpace(timeZoneId))
            {
                return TimeZoneInfo.Utc;
            }

            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.Utc;
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.Utc;
            }
        }
    }
}
