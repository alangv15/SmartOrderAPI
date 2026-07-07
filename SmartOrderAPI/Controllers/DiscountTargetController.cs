using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountTargetController : ControllerBase
    {
        private readonly IDiscountTargetService _service;

        public DiscountTargetController(IDiscountTargetService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DiscountTargetDto>>>> GetAll()
        {
            try
            {
                var targets = await _service.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<DiscountTargetDto>>(targets));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<DiscountTargetDto>>(ex, "Error al obtener los objetivos de descuento."));
            }
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<ApiResponse<DiscountTargetDto>>> GetByCode(string code)
        {
            try
            {
                var dto = await _service.GetByCodeAsync(code);
                if (dto is null)
                    return NotFound(new ApiResponse<DiscountTargetDto>(new Exception("Objetivo de descuento no encontrado.")));

                return Ok(new ApiResponse<DiscountTargetDto>(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<DiscountTargetDto>(ex, "Error al obtener el objetivo de descuento."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Create([FromBody] DiscountTargetDto dto)
        {
            try
            {
                await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByCode), new { code = dto.DiscountTargetCode }, new ApiResponse<string>(dto.DiscountTargetCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(ex, "Error de validación al crear el objetivo."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(ex, "Error inesperado al crear el objetivo."));
            }
        }

        [HttpPut("{code}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(string code, [FromBody] DiscountTargetDto dto)
        {
            if (code != dto.DiscountTargetCode)
                return BadRequest(new ApiResponse<object>(new Exception("El código de la URL no coincide con el del cuerpo.")));

            try
            {
                await _service.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar el objetivo."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el objetivo."));
            }
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(string code)
        {
            try
            {
                await _service.DeleteAsync(code);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el objetivo."));
            }
        }
    }
}
