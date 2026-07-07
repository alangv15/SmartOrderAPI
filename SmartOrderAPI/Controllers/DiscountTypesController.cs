using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountTypesController : ControllerBase
    {
        private readonly IDiscountTypeService _discountTypeService;

        public DiscountTypesController(IDiscountTypeService discountTypeService)
        {
            _discountTypeService = discountTypeService;
        }

        // GET: api/discounttypes
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DiscountTypeDto>>>> GetAll()
        {
            try
            {
                var types = await _discountTypeService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<DiscountTypeDto>>(types));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<DiscountTypeDto>>(ex, "Error al obtener los tipos de descuento."));
            }
        }

        // GET: api/discounttypes/{code}
        [HttpGet("{code}")]
        public async Task<ActionResult<ApiResponse<DiscountTypeDto>>> GetByCode(string code)
        {
            try
            {
                var type = await _discountTypeService.GetByCodeAsync(code);
                if (type == null)
                    return NotFound(new ApiResponse<DiscountTypeDto>(new Exception("Tipo de descuento no encontrado.")));

                return Ok(new ApiResponse<DiscountTypeDto>(type));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<DiscountTypeDto>(ex, "Error al obtener el tipo de descuento."));
            }
        }

        // POST: api/discounttypes
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Create([FromBody] DiscountTypeDto dto)
        {
            try
            {
                await _discountTypeService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByCode), new { code = dto.DiscountTypeCode }, new ApiResponse<string>(dto.DiscountTypeCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(ex, "Error de validación al crear el tipo de descuento."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(ex, "Error inesperado al crear el tipo de descuento."));
            }
        }

        // PUT: api/discounttypes/{code}
        [HttpPut("{code}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(string code, [FromBody] DiscountTypeDto dto)
        {
            if (code != dto.DiscountTypeCode)
                return BadRequest(new ApiResponse<object>(new Exception("El código de la URL no coincide con el del cuerpo.")));

            try
            {
                await _discountTypeService.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar el tipo de descuento."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el tipo de descuento."));
            }
        }

        // DELETE: api/discounttypes/{code}
        [HttpDelete("{code}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(string code)
        {
            try
            {
                await _discountTypeService.DeleteAsync(code);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el tipo de descuento."));
            }
        }
    }
}
