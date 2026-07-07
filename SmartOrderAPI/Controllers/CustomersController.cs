using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CustomerDto>>>> GetAll()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<CustomerDto>>(customers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CustomerDto>>(ex, "Error al obtener los clientes."));
            }
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerDto>>> GetById(int id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                    return NotFound(new ApiResponse<CustomerDto>(new Exception("Cliente no encontrado.")));

                return Ok(new ApiResponse<CustomerDto>(customer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CustomerDto>(ex, "Error al obtener el cliente."));
            }
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CustomerDto dto)
        {
            try
            {
                var id = await _customerService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new ApiResponse<int>(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<int>(ex, "Error de validación al crear el cliente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<int>(ex, "Error inesperado al crear el cliente."));
            }
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] CustomerDto dto)
        {
            if (id != dto.CustomerId)
                return BadRequest(new ApiResponse<object>(new Exception("El ID de la URL no coincide con el del cuerpo.")));

            try
            {
                await _customerService.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar el cliente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el cliente."));
            }
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el cliente."));
            }
        }
    }
}
