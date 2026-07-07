using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserBranchesController : ControllerBase
    {
        private readonly IUserBranchService _userBranchService;

        public UserBranchesController(IUserBranchService userBranchService)
        {
            _userBranchService = userBranchService;
        }

        // GET: api/userbranches/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserBranchDto>>>> GetByUserId(int userId)
        {
            try
            {
                var result = await _userBranchService.GetByUserIdAsync(userId);
                return Ok(new ApiResponse<IEnumerable<UserBranchDto>>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<UserBranchDto>>(ex, "Error al obtener las asignaciones del usuario."));
            }
        }

        // GET: api/userbranches/branch/{branchId}
        [HttpGet("branch/{branchId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserBranchDto>>>> GetByBranchId(int branchId)
        {
            try
            {
                var result = await _userBranchService.GetByBranchIdAsync(branchId);
                return Ok(new ApiResponse<IEnumerable<UserBranchDto>>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<UserBranchDto>>(ex, "Error al obtener los usuarios asignados a la sucursal."));
            }
        }

        // GET: api/userbranches/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserBranchDto>>> GetById(int id)
        {
            try
            {
                var result = await _userBranchService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse<UserBranchDto>(new Exception("Asignación no encontrada.")));

                return Ok(new ApiResponse<UserBranchDto>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UserBranchDto>(ex, "Error al obtener la asignación."));
            }
        }

        // POST: api/userbranches
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Assign([FromBody] UserBranchDto dto)
        {
            try
            {
                var id = await _userBranchService.AssignAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new ApiResponse<int>(id));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiResponse<int>(ex, "Conflicto al asignar el usuario a la sucursal."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<int>(ex, "Error inesperado al asignar el usuario."));
            }
        }

        // DELETE: api/userbranches/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Unassign(int id)
        {
            try
            {
                await _userBranchService.UnassignAsync(id);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al desasignar el usuario."));
            }
        }
    }
}
