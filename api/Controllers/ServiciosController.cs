using api.Models.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ServiciosController : ControllerBase
{
    private readonly IServicioService _service;
    private readonly ILogger<ServiciosController> _logger;

    public ServiciosController(IServicioService service, ILogger<ServiciosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>Retorna todos los servicios (con subtipo Cable o Internet embebido).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServicioResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServicioResponse>>> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Retorna un servicio por su ID.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServicioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicioResponse>> GetById(int id)
    {
        var s = await _service.GetByIdAsync(id);
        if (s is null)
            return NotFound(new { message = $"Servicio con ID {id} no encontrado" });
        return Ok(s);
    }

    /// <summary>Retorna los servicios de un cliente.</summary>
    /// <param name="clienteId">ID del cliente</param>
    [HttpGet("cliente/{clienteId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ServicioResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServicioResponse>>> GetByCliente(int clienteId)
        => Ok(await _service.GetByClienteIdAsync(clienteId));

    /// <summary>
    /// Crea un nuevo servicio. Si TipoServicio es 'Cable', requiere DireccionInstalacion (y opcionalmente PlanCanales).
    /// Si es 'Internet', requiere VelocidadMbps.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ServicioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServicioResponse>> Create([FromBody] ServicioCreateRequest request)
    {
        var s = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = s.IdServicio }, s);
    }

    /// <summary>Actualiza costo y/o lugar de instalación de un servicio.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ServicioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicioResponse>> Update(int id, [FromBody] ServicioUpdateRequest request)
    {
        var s = await _service.UpdateAsync(id, request);
        if (s is null)
            return NotFound(new { message = $"Servicio con ID {id} no encontrado" });
        return Ok(s);
    }

    /// <summary>Suspende (soft-delete) un servicio y su subtipo poniendo estado a 'Suspendido'.</summary>
    /// <param name="id">ID del servicio</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.SoftDeleteAsync(id);
        if (!ok)
            return NotFound(new { message = $"Servicio con ID {id} no encontrado" });
        return NoContent();
    }
}
