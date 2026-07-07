using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentStatusesController : ControllerBase
    {
        private readonly IPaymentStatusService _paymentStatusService;

        public PaymentStatusesController(IPaymentStatusService paymentStatusService)
        {
            _paymentStatusService = paymentStatusService;
        }

        // GET: api/paymentstatuses
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentStatusDto>>>> GetAll()
        {
            try
            {
                var statuses = await _paymentStatusService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<PaymentStatusDto>>(statuses));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<PaymentStatusDto>>(ex, "Error al obtener los estados de pago."));
            }
        }

        // GET: api/paymentstatuses/{code}
        [HttpGet("{code}")]
        public async Task<ActionResult<ApiResponse<PaymentStatusDto>>> GetByCode(string code)
        {
            try
            {
                var status = await _paymentStatusService.GetByCodeAsync(code);
                if (status == null)
                    return NotFound(new ApiResponse<PaymentStatusDto>(new Exception("Estado de pago no encontrado.")));

                return Ok(new ApiResponse<PaymentStatusDto>(status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PaymentStatusDto>(ex, "Error al obtener el estado de pago."));
            }
        }

        // POST: api/paymentstatuses
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Create([FromBody] PaymentStatusDto dto)
        {
            try
            {
                await _paymentStatusService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByCode), new { code = dto.PaymentStatusCode }, new ApiResponse<string>(dto.PaymentStatusCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(ex, "Error de validación al crear el estado de pago."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(ex, "Error inesperado al crear el estado de pago."));
            }
        }

        // PUT: api/paymentstatuses/{code}
        [HttpPut("{code}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(string code, [FromBody] PaymentStatusDto dto)
        {
            if (code != dto.PaymentStatusCode)
                return BadRequest(new ApiResponse<object>(new Exception("El código de la URL no coincide con el del cuerpo.")));

            try
            {
                await _paymentStatusService.UpdateAsync(dto);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<object>(ex, "Error de validación al actualizar el estado de pago."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error inesperado al actualizar el estado de pago."));
            }
        }

        // DELETE: api/paymentstatuses/{code}
        [HttpDelete("{code}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(string code)
        {
            try
            {
                await _paymentStatusService.DeleteAsync(code);
                return Ok(new ApiResponse<object> { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar el estado de pago."));
            }
        }
    }
}
