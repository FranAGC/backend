using api.Models.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/servicios-internet")]
[Authorize]
[Produces("application/json")]
public class ServiciosInternetController : ControllerBase
{
    private readonly IServicioService _service;
    private readonly ILogger<ServiciosInternetController> _logger;

    public ServiciosInternetController(IServicioService service, ILogger<ServiciosInternetController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>Retorna todos los servicios de Internet.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServicioInternetResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServicioInternetResponse>>> GetAll()
        => Ok(await _service.GetAllInternetAsync());

    /// <summary>Retorna un servicio de Internet por ID del servicio.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServicioInternetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicioInternetResponse>> GetById(int id)
    {
        var internet = await _service.GetInternetByIdAsync(id);
        if (internet is null)
            return NotFound(new { message = $"Servicio Internet con ID {id} no encontrado" });
        return Ok(internet);
    }

    /// <summary>Actualiza velocidad y/o meses consecutivos de pago de un servicio Internet.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ServicioInternetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicioInternetResponse>> Update(int id, [FromBody] ServicioInternetUpdateRequest request)
    {
        var internet = await _service.UpdateInternetAsync(id, request);
        if (internet is null)
            return NotFound(new { message = $"Servicio Internet con ID {id} no encontrado" });
        return Ok(internet);
    }

    /// <summary>Suspende (soft-delete) un servicio Internet poniendo estado a 'Suspendido' en internet y en el servicio padre.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.SoftDeleteInternetAsync(id);
        if (!ok)
            return NotFound(new { message = $"Servicio Internet con ID {id} no encontrado" });
        return NoContent();
    }
}
