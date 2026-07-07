using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Request;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<UserDto>>(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<UserDto>>(ex, "Error al obtener los usuarios."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user is null)
                {
                    return NotFound(new ApiResponse<UserDto>(new Exception("Usuario no encontrado.")));
                }

                return Ok(new ApiResponse<UserDto>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UserDto>(ex, "Error al obtener el usuario."));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var user = await _userService.ValidateUserAsync(request.Email, request.Password);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<UserResponseDto>(new Exception("Credenciales inválidas o usuario inactivo")));
                }

                var response = new UserResponseDto
                {
                    UserID = user.UserId,
                    FullName = user.FullName,
                    RoleId = user.RoleId,
                    RoleCode = user.RoleCode,
                    RoleName = user.RoleName,
                    Permissions = await _userService.GetPermissionCodesByRoleIdAsync(user.RoleId)
                };

                return Ok(new ApiResponse<UserResponseDto>(response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UserResponseDto>(ex, "Error al validar el usuario."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] UserDto user)
        {
            try
            {
                var success = await _userService.CreateAsync(user);
                if (!success)
                {
                    return BadRequest(new ApiResponse<object>(new Exception("Algo salió mal al crear el usuario.")));
                }

                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al crear el usuario."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al crear el usuario."));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] UserDto dto)
        {
            if (id != dto.UserId)
            {
                return BadRequest(new ApiResponse<object>(new Exception("El ID de la URL no coincide con el del cuerpo.")));
            }

            try
            {
                await _userService.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar el usuario."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el usuario."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el usuario."));
            }
        }
    }
}
