using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchesController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        // GET: api/branches
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchDto>>>> GetAll()
        {
            try
            {
                var branches = await _branchService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<BranchDto>>(branches));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<BranchDto>>(ex, "Error al obtener las sucursales."));
            }
        }

        // GET: api/branches/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BranchDto>>> GetById(int id)
        {
            try
            {
                var branch = await _branchService.GetByIdAsync(id);
                if (branch == null)
                    return NotFound(new ApiResponse<BranchDto>(new Exception("Sucursal no encontrada.")));

                return Ok(new ApiResponse<BranchDto>(branch));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<BranchDto>(ex, "Error al obtener la sucursal."));
            }
        }

        // POST: api/branches
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] BranchDto dto)
        {
            try
            {
                var id = await _branchService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new ApiResponse<int>(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<int>(ex, "Error de validación al crear la sucursal."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<int>(ex, "Error inesperado al crear la sucursal."));
            }
        }

        // PUT: api/branches/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] BranchDto dto)
        {
            if (id != dto.BranchId)
                return BadRequest(new ApiResponse<object>(new Exception("El ID de la URL no coincide con el del cuerpo.")));

            try
            {
                await _branchService.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }

            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar la sucursal."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar la sucursal."));
            }
        }

        // DELETE: api/branches/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                await _branchService.DeleteAsync(id);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar la sucursal."));
            }
        }
    }
}
