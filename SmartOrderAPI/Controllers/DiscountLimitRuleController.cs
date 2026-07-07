using Microsoft.AspNetCore.Mvc;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Entities.Models;
using SmartOrderAPI.Entities.Response;

namespace SmartOrderAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountLimitRuleController : ControllerBase
{
    private readonly IDiscountLimitRuleService _service;

    public DiscountLimitRuleController(IDiscountLimitRuleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<DiscountLimitRuleDto>>>> GetAll()
    {
        try
        {
            var rules = await _service.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<DiscountLimitRuleDto>>(rules));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<IEnumerable<DiscountLimitRuleDto>>(ex, "Error al obtener las reglas de limite de descuento."));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DiscountLimitRuleDto>>> GetById(int id)
    {
        try
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null
                ? NotFound(new ApiResponse<DiscountLimitRuleDto>(new Exception("Regla de limite no encontrada.")))
                : Ok(new ApiResponse<DiscountLimitRuleDto>(dto));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<DiscountLimitRuleDto>(ex, "Error al obtener la regla de limite de descuento."));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] DiscountLimitRuleDto dto)
    {
        try
        {
            await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.DiscountLimitRuleId }, new ApiResponse<int>(dto.DiscountLimitRuleId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<int>(ex, "Error al crear la regla de limite de descuento."));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] DiscountLimitRuleDto dto)
    {
        if (id != dto.DiscountLimitRuleId)
        {
            return BadRequest(new ApiResponse<object>(new Exception("El ID no coincide.")));
        }

        try
        {
            await _service.UpdateAsync(dto);
            return Ok(new ApiResponse<object> { Success = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>(ex, "Error al actualizar la regla de limite de descuento."));
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
            return StatusCode(500, new ApiResponse<object>(ex, "Error al eliminar la regla de limite de descuento."));
        }
    }
}
