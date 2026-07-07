using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<RoleDto>>(roles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<RoleDto>>(ex, "Error al obtener los roles."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> GetById(int id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                if (role is null)
                {
                    return NotFound(new ApiResponse<RoleDto>(new Exception("Rol no encontrado.")));
                }

                return Ok(new ApiResponse<RoleDto>(role));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RoleDto>(ex, "Error al obtener el rol."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] RoleDto role)
        {
            try
            {
                await _roleService.CreateAsync(role);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validacion al crear el rol."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al crear el rol."));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] RoleDto role)
        {
            if (id != role.RoleId)
            {
                return BadRequest(new ApiResponse<object>(new Exception("El ID de la URL no coincide con el del cuerpo.")));
            }

            try
            {
                await _roleService.UpdateAsync(role);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validacion al actualizar el rol."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el rol."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Deactivate(int id)
        {
            try
            {
                await _roleService.DeactivateAsync(id);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al desactivar el rol."));
            }
        }
    }
}
