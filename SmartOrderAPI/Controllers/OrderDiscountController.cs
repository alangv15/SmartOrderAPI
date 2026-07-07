using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDiscountController : ControllerBase
    {
        private readonly IOrderDiscountService _service;

        public OrderDiscountController(IOrderDiscountService service)
        {
            _service = service;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDiscountDto>>>> GetByOrderId(int orderId)
        {
            try
            {
                var discounts = await _service.GetByOrderIdAsync(orderId);
                return Ok(new ApiResponse<IEnumerable<OrderDiscountDto>>(discounts));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<OrderDiscountDto>>(ex, "Error al obtener los descuentos del pedido."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Add([FromBody] OrderDiscountDto dto)
        {
            try
            {
                await _service.AddAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al agregar el descuento."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al agregar el descuento."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el descuento."));
            }
        }
    }
}
