using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Reports.Service;
using SmartOrderAPI.Entities.Reports.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers.Reports
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("daily-sales")]
        public async Task<ActionResult<ApiResponse<DailySalesReportDto>>> GetDailySales([FromQuery] DateTime? date)
        {
            try
            {
                var report = await _reportService.GetDailySalesReportAsync(date);
                return Ok(new ApiResponse<DailySalesReportDto>(report));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<DailySalesReportDto>(ex, "Error al obtener el reporte diario de ventas."));
            }
        }

        [HttpGet("sales-summary")]
        public async Task<ActionResult<ApiResponse<SalesSummaryReportDto>>> GetSalesSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var report = await _reportService.GetSalesSummaryReportAsync(startDate, endDate);
                return Ok(new ApiResponse<SalesSummaryReportDto>(report));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SalesSummaryReportDto>(ex, "Error al obtener el reporte acumulado de ventas."));
            }
        }
    }
}
