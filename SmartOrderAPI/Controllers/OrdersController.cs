using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetAll()
        {
            try
            {
                var orders = await _orderService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<OrderDto>>(orders));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<OrderDto>>(ex, "Error al obtener los pedidos."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetById(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                    return NotFound(new ApiResponse<OrderDto>(new Exception("Pedido no encontrado.")));

                return Ok(new ApiResponse<OrderDto>(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<OrderDto>(ex, "Error al obtener el pedido."));
            }
        }

        [HttpGet("sales/{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetInStoreSaleById(int id)
        {
            try
            {
                var order = await _orderService.GetInStoreSaleByIdAsync(id);
                if (order == null)
                    return NotFound(new ApiResponse<OrderDto>(new Exception("Venta de mostrador no encontrada.")));

                return Ok(new ApiResponse<OrderDto>(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<OrderDto>(ex, "Error al obtener la venta de mostrador."));
            }
        }

        [HttpGet("custom/active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetActiveCustomOrders()
        {
            try
            {
                var orders = await _orderService.GetActiveCustomOrdersAsync();
                return Ok(new ApiResponse<IEnumerable<OrderDto>>(orders));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<OrderDto>>(ex, "Error al obtener los pedidos vigentes."));
            }
        }

        [HttpGet("custom/summary")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderSummaryDto>>>> GetCustomOrderSummaries(
            [FromQuery] DateTime startUtc,
            [FromQuery] DateTime endExclusiveUtc)
        {
            try
            {
                var orders = await _orderService.GetCustomOrderSummariesAsync(startUtc, endExclusiveUtc);
                return Ok(new ApiResponse<IEnumerable<OrderSummaryDto>>(orders));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<OrderSummaryDto>>(ex, "Rango de fechas invalido."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<OrderSummaryDto>>(ex, "Error al obtener el resumen de pedidos."));
            }
        }

        [HttpGet("custom/{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetCustomOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetCustomOrderByIdAsync(id);
                if (order == null)
                    return NotFound(new ApiResponse<OrderDto>(new Exception("Pedido no encontrado.")));

                return Ok(new ApiResponse<OrderDto>(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<OrderDto>(ex, "Error al obtener el pedido."));
            }
        }

        [HttpPatch("custom/{id}/status")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateCustomOrderStatus(int id, [FromBody] OrderStatusUpdateDto dto)
        {
            try
            {
                await _orderService.UpdateStatusAsync(id, dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validacion al actualizar estatus."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar estatus del pedido."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] OrderDto dto)
        {
            try
            {
                var id = await _orderService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new ApiResponse<int>(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<int>(ex, "Error de validación al crear el pedido."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<int>(ex, "Error inesperado al crear el pedido."));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] OrderDto dto)
        {
            if (id != dto.OrderId)
                return BadRequest(new ApiResponse<object>(new Exception("El ID de la URL no coincide con el del cuerpo.")));

            try
            {
                await _orderService.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar el pedido."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el pedido."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Cancel(int id)
        {
            try
            {
                await _orderService.CancelAsync(id);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al cancelar el pedido."));
            }
        }
    }
}
