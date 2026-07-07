using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAll()
        {
            try
            {
                var categories = await _service.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<CategoryDto>>(categories));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CategoryDto>>(ex, "Error al obtener las categorías."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetById(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto is null)
                    return NotFound(new ApiResponse<CategoryDto>(new Exception("Categoría no encontrada.")));

                return Ok(new ApiResponse<CategoryDto>(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CategoryDto>(ex, "Error al obtener la categoría."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CategoryDto dto)
        {
            try
            {
                await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.CategoryId }, new ApiResponse<int>(dto.CategoryId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<int>(ex, "Error de validación al crear la categoría."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<int>(ex, "Error inesperado al crear la categoría."));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] CategoryDto dto)
        {
            if (id != dto.CategoryId)
                return BadRequest(new ApiResponse<object>(new Exception("El ID de la URL no coincide con el del cuerpo.")));

            try
            {
                await _service.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar la categoría."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar la categoría."));
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
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar la categoría."));
            }
        }
    }
}
