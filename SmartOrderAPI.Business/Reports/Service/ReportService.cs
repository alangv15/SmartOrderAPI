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

        public async Task<MonthlyProfitReportDto> GetMonthlyProfitReportAsync(DateTime? endMonth, int months)
        {
            if (months is < 1 or > 24)
                throw new ArgumentOutOfRangeException(nameof(months), "El numero de meses debe estar entre 1 y 24.");

            return await _reportRepository.GetMonthlyProfitReportAsync(endMonth, months);
        }
    }
}
