using SmartOrderAPI.Entities.Reports.Models;

namespace SmartOrderAPI.Business.Reports.Service
{
    public interface IReportService
    {
        Task<DailySalesReportDto> GetDailySalesReportAsync(DateTime? date);
        Task<SalesSummaryReportDto> GetSalesSummaryReportAsync(DateTime? startDate, DateTime? endDate);
    }
}
