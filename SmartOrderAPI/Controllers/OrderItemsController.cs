using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        // GET: api/orderitems/order/{orderId}
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderItemDto>>>> GetByOrderId(int orderId)
        {
            try
            {
                var items = await _orderItemService.GetByOrderIdAsync(orderId);
                return Ok(new ApiResponse<IEnumerable<OrderItemDto>>(items));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<OrderItemDto>>(ex, "Error al obtener los ítems del pedido."));
            }
        }

        // GET: api/orderitems/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderItemDto>>> GetById(int id)
        {
            try
            {
                var item = await _orderItemService.GetByIdAsync(id);
                if (item == null)
                    return NotFound(new ApiResponse<OrderItemDto>(new Exception("Ítem de pedido no encontrado.")));

                return Ok(new ApiResponse<OrderItemDto>(item));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<OrderItemDto>(ex, "Error al obtener el ítem del pedido."));
            }
        }

        // POST: api/orderitems
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] OrderItemDto dto)
        {
            try
            {
                var id = await _orderItemService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new ApiResponse<int>(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<int>(ex, "Error de validación al crear el ítem."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<int>(ex, "Error inesperado al crear el ítem."));
            }
        }

        // PUT: api/orderitems/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] OrderItemDto dto)
        {
            if (id != dto.OrderItemId)
                return BadRequest(new ApiResponse<object>(new Exception("El ID de la URL no coincide con el del cuerpo.")));

            try
            {
                await _orderItemService.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar el ítem."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el ítem."));
            }
        }

        // DELETE: api/orderitems/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                await _orderItemService.DeleteAsync(id);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el ítem."));
            }
        }
    }
}
