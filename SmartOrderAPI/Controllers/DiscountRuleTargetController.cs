using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountRuleTargetController : ControllerBase
    {
        private readonly IDiscountRuleTargetService _service;

        public DiscountRuleTargetController(IDiscountRuleTargetService service)
        {
            _service = service;
        }

        [HttpGet("rule/{discountRuleId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DiscountRuleTargetDto>>>> GetByRuleId(int discountRuleId)
        {
            try
            {
                var targets = await _service.GetByRuleIdAsync(discountRuleId);
                return Ok(new ApiResponse<IEnumerable<DiscountRuleTargetDto>>(targets));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<DiscountRuleTargetDto>>(ex, "Error al obtener los objetivos de la regla de descuento."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Add([FromBody] DiscountRuleTargetDto dto)
        {
            try
            {
                await _service.AddAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al agregar el objetivo."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al agregar el objetivo."));
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
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el objetivo."));
            }
        }
    }
}
