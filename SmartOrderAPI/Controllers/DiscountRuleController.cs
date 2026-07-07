using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Orders.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountRuleController : ControllerBase
    {
        private readonly IDiscountRuleService _service;

        public DiscountRuleController(IDiscountRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DiscountRuleDto>>>> GetAll()
        {
            try
            {
                var rules = await _service.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<DiscountRuleDto>>(rules));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<DiscountRuleDto>>(ex, "Error al obtener las reglas de descuento."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DiscountRuleDto>>> GetById(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto is null)
                    return NotFound(new ApiResponse<DiscountRuleDto>(new Exception("Regla de descuento no encontrada.")));

                return Ok(new ApiResponse<DiscountRuleDto>(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<DiscountRuleDto>(ex, "Error al obtener la regla de descuento."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] DiscountRuleDto dto)
        {
            try
            {
                await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.DiscountRuleId }, new ApiResponse<int>(dto.DiscountRuleId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<int>(ex, "Error de validación al crear la regla de descuento."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<int>(ex, "Error inesperado al crear la regla de descuento."));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] DiscountRuleDto dto)
        {
            if (id != dto.DiscountRuleId)
                return BadRequest(new ApiResponse<object>(new Exception("El ID de la URL no coincide con el del cuerpo.")));

            try
            {
                await _service.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar la regla de descuento."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar la regla de descuento."));
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
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar la regla de descuento."));
            }
        }
    }
}
