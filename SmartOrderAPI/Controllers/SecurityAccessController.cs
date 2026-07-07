using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Request;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/security-access")]
    public class SecurityAccessController : ControllerBase
    {
        private readonly ISecurityAccessService _securityAccessService;

        public SecurityAccessController(ISecurityAccessService securityAccessService)
        {
            _securityAccessService = securityAccessService;
        }

        [HttpGet("{roleId}")]
        public async Task<ActionResult<ApiResponse<RoleAccessDto>>> GetByRole(int roleId)
        {
            try
            {
                var access = await _securityAccessService.GetRoleAccessAsync(roleId);
                if (access is null)
                {
                    return NotFound(new ApiResponse<RoleAccessDto>(new Exception("Rol no encontrado.")));
                }

                return Ok(new ApiResponse<RoleAccessDto>(access));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RoleAccessDto>(ex, "Error al obtener los accesos del rol."));
            }
        }

        [HttpPut("{roleId}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int roleId, [FromBody] RoleAccessUpdateDto request)
        {
            try
            {
                await _securityAccessService.UpdateRolePermissionsAsync(roleId, request.PermissionIds);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validacion al actualizar los accesos del rol."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar los accesos del rol."));
            }
        }
    }
}
