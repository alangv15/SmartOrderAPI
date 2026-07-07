using SmartOrderAPI.Data.Reports.Repositories;
using SmartOrderAPI.Entities.Reports.Models;

namespace SmartOrderAPI.Business.Reports.Service
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<DailySalesReportDto> GetDailySalesReportAsync(DateTime? date)
        {
            return await _reportRepository.GetDailySalesReportAsync(date);
        }

        public async Task<SalesSummaryReportDto> GetSalesSummaryReportAsync(DateTime? startDate, DateTime? endDate)
        {
            return await _reportRepository.GetSalesSummaryReportAsync(startDate, endDate);
        }
    }
}
