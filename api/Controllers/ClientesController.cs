using api.Models.DTOs;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(IClienteService service, ILogger<ClientesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>Retorna todos los clientes con sus servicios.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClienteResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClienteResponse>>> GetAll()
    {
        var clientes = await _service.GetAllAsync();
        return Ok(clientes);
    }

    /// <summary>Retorna un cliente por su ID.</summary>
    /// <param name="id">ID del cliente</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteResponse>> GetById(int id)
    {
        var cliente = await _service.GetByIdAsync(id);
        if (cliente is null)
            return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
        return Ok(cliente);
    }

    /// <summary>Crea un nuevo cliente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteResponse>> Create([FromBody] ClienteCreateRequest request)
    {
        var cliente = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = cliente.IdCliente }, cliente);
    }

    /// <summary>Actualiza los datos de un cliente.</summary>
    /// <param name="id">ID del cliente</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteResponse>> Update(int id, [FromBody] ClienteUpdateRequest request)
    {
        var cliente = await _service.UpdateAsync(id, request);
        if (cliente is null)
            return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
        return Ok(cliente);
    }

    /// <summary>Suspende (soft-delete) un cliente cambiando su estado a 'Suspendido'.</summary>
    /// <param name="id">ID del cliente</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.SoftDeleteAsync(id);
        if (!success)
            return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
        return NoContent();
    }
}
