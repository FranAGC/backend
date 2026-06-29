using api.Models.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/servicios-cable")]
[Authorize]
[Produces("application/json")]
public class ServiciosCableController : ControllerBase
{
    private readonly IServicioService _service;
    private readonly ILogger<ServiciosCableController> _logger;

    public ServiciosCableController(IServicioService service, ILogger<ServiciosCableController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>Retorna todos los servicios de Cable.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServicioCableResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServicioCableResponse>>> GetAll()
        => Ok(await _service.GetAllCableAsync());

    /// <summary>Retorna un servicio de Cable por ID del servicio.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServicioCableResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicioCableResponse>> GetById(int id)
    {
        var cable = await _service.GetCableByIdAsync(id);
        if (cable is null)
            return NotFound(new { message = $"Servicio Cable con ID {id} no encontrado" });
        return Ok(cable);
    }

    /// <summary>Actualiza dirección y/o plan de canales de un servicio Cable.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ServicioCableResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicioCableResponse>> Update(int id, [FromBody] ServicioCableUpdateRequest request)
    {
        var cable = await _service.UpdateCableAsync(id, request);
        if (cable is null)
            return NotFound(new { message = $"Servicio Cable con ID {id} no encontrado" });
        return Ok(cable);
    }

    /// <summary>Suspende (soft-delete) un servicio Cable poniendo estado a 'Suspendido' en cable y en el servicio padre.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.SoftDeleteCableAsync(id);
        if (!ok)
            return NotFound(new { message = $"Servicio Cable con ID {id} no encontrado" });
        return NoContent();
    }
}
