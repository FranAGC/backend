using api.Data.Repositories;
using api.Models.Domain;
using api.Models.DTOs;
using api.Services.Interfaces;

namespace api.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repo;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(IClienteRepository repo, ILogger<ClienteService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<ClienteResponse>> GetAllAsync()
    {
        _logger.LogInformation("Obteniendo todos los clientes");
        var clientes = await _repo.GetAllWithServiciosAsync();
        return clientes.Select(Map);
    }

    public async Task<ClienteResponse?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Obteniendo cliente ID: {Id}", id);
        var cliente = await _repo.GetByIdWithServiciosAsync(id);
        return cliente is null ? null : Map(cliente);
    }

    public async Task<ClienteResponse> CreateAsync(ClienteCreateRequest request)
    {
        _logger.LogInformation("Creando cliente: {Codigo}", request.Codigo);

        if (await _repo.GetByCodigoAsync(request.Codigo) is not null)
            throw new InvalidOperationException($"Ya existe un cliente con el código '{request.Codigo}'");

        if (await _repo.GetByCorreoAsync(request.Correo) is not null)
            throw new InvalidOperationException($"Ya existe un cliente con el correo '{request.Correo}'");

        var cliente = new Cliente
        {
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            Direccion = request.Direccion,
            Correo = request.Correo,
            Telefono = request.Telefono,
            Estado = "Activo",
            FechaAlta = DateOnly.FromDateTime(DateTime.Now),
            CreadoEn = DateTime.Now
        };

        var created = await _repo.AddAsync(cliente);
        _logger.LogInformation("Cliente creado ID: {Id}", created.IdCliente);
        return Map(created);
    }

    public async Task<ClienteResponse?> UpdateAsync(int id, ClienteUpdateRequest request)
    {
        _logger.LogInformation("Actualizando cliente ID: {Id}", id);
        var cliente = await _repo.GetByIdWithServiciosAsync(id);
        if (cliente is null) return null;

        if (!string.IsNullOrWhiteSpace(request.Nombre))
            cliente.Nombre = request.Nombre;

        if (!string.IsNullOrWhiteSpace(request.Direccion))
            cliente.Direccion = request.Direccion;

        if (!string.IsNullOrWhiteSpace(request.Correo))
        {
            var existing = await _repo.GetByCorreoAsync(request.Correo);
            if (existing is not null && existing.IdCliente != id)
                throw new InvalidOperationException("El correo ya está en uso por otro cliente");
            cliente.Correo = request.Correo;
        }

        if (!string.IsNullOrWhiteSpace(request.Telefono))
            cliente.Telefono = request.Telefono;

        await _repo.UpdateAsync(cliente);
        _logger.LogInformation("Cliente actualizado ID: {Id}", id);
        return Map(cliente);
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        _logger.LogInformation("Soft-delete cliente ID: {Id}", id);
        var cliente = await _repo.GetByIdAsync(id);
        if (cliente is null) return false;

        cliente.Estado = "Suspendido";
        await _repo.UpdateAsync(cliente);
        _logger.LogInformation("Cliente suspendido ID: {Id}", id);
        return true;
    }

    // ── Mapping ───────────────────────────────────────────────────────────────
    private static ClienteResponse Map(Cliente c) => new()
    {
        IdCliente = c.IdCliente,
        Codigo = c.Codigo,
        Nombre = c.Nombre,
        FechaAlta = c.FechaAlta,
        Direccion = c.Direccion,
        Correo = c.Correo,
        Telefono = c.Telefono,
        Estado = c.Estado,
        CreadoEn = c.CreadoEn,
        Servicios = c.Servicios.Select(MapServicio).ToList()
    };

    private static ServicioResponse MapServicio(Servicio s) => new()
    {
        IdServicio = s.IdServicio,
        CodigoServicio = s.CodigoServicio,
        IdCliente = s.IdCliente,
        TipoServicio = s.TipoServicio,
        CostoMensualBase = s.CostoMensualBase,
        LugarInstalacion = s.LugarInstalacion,
        FechaContratacion = s.FechaContratacion,
        Estado = s.Estado,
        Cable = s.ServicioCable is null ? null : new ServicioCableResponse
        {
            IdServicio = s.ServicioCable.IdServicio,
            DireccionInstalacion = s.ServicioCable.DireccionInstalacion,
            PlanCanales = s.ServicioCable.PlanCanales,
            Estado = s.ServicioCable.Estado,
            CodigoServicio = s.CodigoServicio,
            EstadoServicioPadre = s.Estado
        },
        Internet = s.ServicioInternet is null ? null : new ServicioInternetResponse
        {
            IdServicio = s.ServicioInternet.IdServicio,
            VelocidadMbps = s.ServicioInternet.VelocidadMbps,
            MesesConsecutivosPagos = s.ServicioInternet.MesesConsecutivosPagos,
            Estado = s.ServicioInternet.Estado,
            CodigoServicio = s.CodigoServicio,
            EstadoServicioPadre = s.Estado
        }
    };
}
