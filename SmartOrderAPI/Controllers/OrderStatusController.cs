using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatusService;

        public OrderStatusController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        // GET: api/orderstatuses
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderStatusDto>>>> GetAll()
        {
            try
            {
                var statuses = await _orderStatusService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<OrderStatusDto>>(statuses));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<OrderStatusDto>>(ex, "Error al obtener los estados de pedido."));
            }
        }

        // GET: api/orderstatuses/{code}
        [HttpGet("{code}")]
        public async Task<ActionResult<ApiResponse<OrderStatusDto>>> GetByCode(string code)
        {
            try
            {
                var status = await _orderStatusService.GetByCodeAsync(code);
                if (status == null)
                    return NotFound(new ApiResponse<OrderStatusDto>(new Exception("Estado de pedido no encontrado.")));

                return Ok(new ApiResponse<OrderStatusDto>(status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<OrderStatusDto>(ex, "Error al obtener el estado de pedido."));
            }
        }

        // POST: api/orderstatuses
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Create([FromBody] OrderStatusDto dto)
        {
            try
            {
                await _orderStatusService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByCode), new { code = dto.OrderStatusCode }, new ApiResponse<string>(dto.OrderStatusCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(ex, "Error de validación al crear el estado de pedido."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(ex, "Error inesperado al crear el estado de pedido."));
            }
        }

        // PUT: api/orderstatuses/{code}
        [HttpPut("{code}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(string code, [FromBody] OrderStatusDto dto)
        {
            if (code != dto.OrderStatusCode)
                return BadRequest(new ApiResponse<object>(new Exception("El código de la URL no coincide con el del cuerpo.")));

            try
            {
                await _orderStatusService.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar el estado de pedido."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el estado de pedido."));
            }
        }

        // DELETE: api/orderstatuses/{code}
        [HttpDelete("{code}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(string code)
        {
            try
            {
                await _orderStatusService.DeleteAsync(code);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el estado de pedido."));
            }
        }
    }
}
