using SmartOrderAPI.Entities.Reports.Models;

namespace SmartOrderAPI.Data.Reports.Repositories
{
    public interface IReportRepository
    {
        Task<DailySalesReportDto> GetDailySalesReportAsync(DateTime? date);
        Task<SalesSummaryReportDto> GetSalesSummaryReportAsync(DateTime? startDate, DateTime? endDate);
        Task<MonthlyProfitReportDto> GetMonthlyProfitReportAsync(DateTime? endMonth, int months);
    }
}
